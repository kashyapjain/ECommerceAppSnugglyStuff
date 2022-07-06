using Newtonsoft.Json;
using SnugglyStuff.Controller;
using SnugglyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnugglyStuff.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryView : ContentPage
    {
        public CategoryView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            InitSlider();
            InitCategoryImages();
        }

        public void InitCategoryImages()
        {
            var categories = ApiController.GetCategories();

            if (categories != null)
            {

                var CategoryIndex = 0;

                for (int rowIndex = 0; rowIndex < categories.Count / 2 + 1; rowIndex++)
                {
                    for (int columnIndex = 0; columnIndex < 2; columnIndex++)
                    {
                        if (CategoryIndex >= categories.Count)
                        {
                            break;
                        }

                        var category = categories[CategoryIndex];

                        CategoryIndex += 1;

                        //Label

                        var label = new Label
                        {
                            Text = category.CategoryName,
                            TextColor = Color.LightGray,
                            VerticalOptions = LayoutOptions.End,
                            FontAttributes = FontAttributes.Bold,
                        };

                        AbsoluteLayout.SetLayoutBounds(label, new Rectangle(0, 1, 1, 50));

                        AbsoluteLayout.SetLayoutFlags(label,
                        AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.WidthProportional);

                        //Image

                        UriImageSource uriImage = new UriImageSource
                        {
                            Uri= new Uri(category.Image),
                            CachingEnabled = false
                        };

                        var image = new Image
                        {
                            Source = uriImage
                        };
                        //Absoloute Layout

                        var absolouteLayout = new AbsoluteLayout
                        {
                            Padding = 5,
                            HeightRequest =150
                        };
                        //Absoloute Layout Add Children

                        absolouteLayout.Children.Add(image);
                        absolouteLayout.Children.Add(label);

                        //Register Tap Gesture on Absoloute Layout

                        var tapGestureRecognizer = new TapGestureRecognizer();

                        tapGestureRecognizer.Tapped += (s, e) =>
                        {
                            OnTapped(s, e, category);
                        };

                        absolouteLayout.GestureRecognizers.Add(tapGestureRecognizer);

                        //Add Children to Grid View

                        CategoryGridLayout.Children.Add(absolouteLayout, columnIndex, rowIndex);
                    }
                }
            }
            else
            {
                DisplayAlert("Message", "Server Error", "ok");
            }
        }

        public void OnTapped(object s,EventArgs e,Category category)
        {
            ItemsView.category = category;

            var route = $"{nameof(ItemsView)}";

            Shell.Current.GoToAsync(route);
        }

        public void InitSlider()
        {

            var sliders = ApiController.GetSliders();

            MainCarouselView.ItemsSource = sliders;

            Device.StartTimer(TimeSpan.FromSeconds(5), (Func<bool>)(() =>
            {
                MainCarouselView.Position = (MainCarouselView.Position + 1) % sliders.Count;
                return true;
            }));

        }

        public async void SliderImageTapped(object sender, EventArgs e)
        {
            var slider = ((Image)sender).BindingContext as Models.Sliders;

            ItemsView.category = new Category
            {
                ID = Convert.ToInt32(slider.CategoryID)
            };

            var route = $"{nameof(ItemsView)}";

            await Shell.Current.GoToAsync(route);
        }

        private void Logout(object sender, EventArgs e)
        {
            Application.Current.Properties["ID"] = null;
        }
    }
}