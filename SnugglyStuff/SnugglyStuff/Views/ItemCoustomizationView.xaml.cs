using Plugin.ImageEdit;
using SnugglyStuff.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnugglyStuff.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemCoustomizationView : ContentPage
    {
        public static Item item { get; set; }
        public ItemCoustomizationView()
        {
            InitializeComponent();

            Image fi = new Image();
            Image si = new Image();

            fi.Source = item.ImageSrc;
            si.Source = Path.Combine(FileSystem.AppDataDirectory,
                                 item.ID.ToString() + "Second.png");

            FirstImage.Source =  item.ImageSrc;
            SecondImage.Source = Path.Combine(FileSystem.AppDataDirectory,
                                 item.ID.ToString() + "Second.png");

            FirstImage.WidthRequest = fi.Width;
            SecondImage.WidthRequest = si.Width;

            FirstImage.HeightRequest = fi.Height;
            SecondImage.HeightRequest = si.Height;
        }

        private async void PlaceOrder(object sender, EventArgs e)
        {
            //Take ScreenShot

            var screenshot = await Screenshot.CaptureAsync();
            var SSstream = await screenshot.OpenReadAsync();

            //Crop ScreenShot

            var image = await CrossImageEdit.Current.CreateImageAsync(SSstream);

            int width = image.Width;
            int height = Convert.ToInt32(FirstImage.Height * 1.8);
            
            int x = 0;
            int y = 165;

            image.Crop(x,y,width,height);

            //Save Croped ScreenShot

            var bytes = image.ToPng();

            var path = Path.Combine(FileSystem.AppDataDirectory,
                                item.ID.ToString() + "Final.png");

            File.WriteAllBytes(path,bytes);

            //Assign Values to Place Order View

            item.ImageSrc = path;

            PlaceOrderView.item = item;

            //Navigate to Place Order View

            var route = $"{nameof(PlaceOrderView)}";

            Shell.Current.GoToAsync(route);
        }

        private void Up(object sender, EventArgs e)
        {
            SecondImage.TranslationY = SecondImage.TranslationY - 5;
        }

        private void Down(object sender, EventArgs e)
        {
            SecondImage.TranslationY = SecondImage.TranslationY + 5;
        }

        private void Left(object sender, EventArgs e)
        {
            SecondImage.TranslationX = SecondImage.TranslationX - 5;
        }

        private void Right(object sender, EventArgs e)
        {
            SecondImage.TranslationX = SecondImage.TranslationX + 5;
        }

        private void Increase(object sender, EventArgs e)
        {
            SecondImage.WidthRequest = SecondImage.Width + 5;
            SecondImage.HeightRequest = SecondImage.Height + 5;

            FirstImage.WidthRequest = FirstImage.Width;
            FirstImage.HeightRequest = FirstImage.Height;
        }

        private void Decrease(object sender, EventArgs e)
        {
            SecondImage.WidthRequest = SecondImage.Width - 5;
            SecondImage.HeightRequest = SecondImage.Height - 5;

            FirstImage.WidthRequest = FirstImage.Width;
            FirstImage.HeightRequest = FirstImage.Height;
        }
    }
}