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
    public partial class ItemsView : ContentPage
    {
        public static Category category { get; set; }
        public ItemsView()
        {
            InitializeComponent();

            var categories = ApiController.GetCategories();

            var _category = categories.Find(i => i.ID == category.ID);

            ItemsViewContentPage.Title = _category.CategoryName + " Page";

            Heading.Text = "Home / " + _category.CategoryName;

            InitItems();
        }

        void InitItems()
        {
            var items = ApiController.GetItems(category.ID.ToString());

            var ItemIndex = 0;

            for (int rowIndex = 0; rowIndex < items.Count / 2 + 1; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 2; columnIndex++)
                {
                    if (ItemIndex >= items.Count)
                    {
                        break;
                    }

                    var item = items[ItemIndex];

                    ItemIndex += 1;

                    //Stack Layout

                    var stacklayout = new StackLayout
                    {
                        Padding = 5,
                        HeightRequest = 350,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };

                    //Label

                    var label = new Label
                    {
                        Text = item.Description,
                        TextColor = Color.Black,
                        HorizontalOptions= LayoutOptions.CenterAndExpand,
                        FontSize = 20,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                    };

                    //Image

                    UriImageSource uriImage = new UriImageSource
                    {
                        Uri = new Uri(item.ImageSrc),
                        CachingEnabled = false
                    };

                    var image = new Image
                    {
                        Source = uriImage
                    };

                    //Stack Layout Add Children

                    stacklayout.Children.Add(image);
                    stacklayout.Children.Add(label);

                    //Register Tap Gesture on Absoloute Layout

                    var tapGestureRecognizer = new TapGestureRecognizer();

                    tapGestureRecognizer.Tapped += (s, e) =>
                    {
                        OnTapped(s, e, item);
                    };

                    stacklayout.GestureRecognizers.Add(tapGestureRecognizer);

                    //Add Children to Grid View

                    CategoryGridLayout.Children.Add(stacklayout, columnIndex, rowIndex);
                }
            }
        }

        private void OnTapped(object s, EventArgs e, Item item)
        {
            ItemView.item = item;


            var route = $"{nameof(ItemView)}";

            Shell.Current.GoToAsync(route);
        }
    }
}