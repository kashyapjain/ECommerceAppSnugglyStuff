using SnugglyStuff.Controller;
using SnugglyStuff.Models;
using SnugglyStuff.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnugglyStuff.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaceOrderView : ContentPage
    {
        public static Item item { get; set; }
        static User user;
        public PlaceOrderView()
        {
            InitializeComponent();

            try
            {
                ImageSrc.Source = item.ImageSrc;
                Description.Text = item.Description;
                Price.Text = item.Price;
                var temp = item.Price.Substring(1, item.Price.Length -1);
                lblTotal.Text = "$" + (Convert.ToInt32(temp) + 5).ToString();

                var id = Application.Current.Properties["ID"];
                user = ApiController.GetUserdetails(id.ToString());

                Email.Text = user.Email;
                Address.Text = user.Address;
                PhoneNo.Text = user.PhoneNo;
            }
            catch(Exception e)
            {
                Login();
            }
        }
        public async void Login()
        {
            await DisplayAlert("Message", "Please Login", "ok");

            Shell.Current.Navigation.PopAsync();

            var route = $"{nameof(LoginPage)}";

            Shell.Current.GoToAsync(route);
        }

        private void PlaceOrder(object sender, EventArgs e)
        {
            //Convert Date According to yyyy-mm-dd mm sss

            var DateString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var fileStream = File.OpenRead(item.ImageSrc);
            Stream stream = new MemoryStream();
            fileStream.CopyTo(stream);

            stream.Position = 0;

            // Now read s into a byte buffer with a little padding.
            byte[] bytes = new byte[stream.Length + 10];
            int numBytesToRead = (int)stream.Length;
            int numBytesRead = 0;
            do
            {
                // Read may return anything from 0 to 10.
                int n = stream.Read(bytes, numBytesRead, 10);
                numBytesRead += n;
                numBytesToRead -= n;
            } while (numBytesToRead > 0);

            var ImageString = Convert.ToBase64String(bytes);

            Order order = new Order
            {
                DateAndTime = DateString,
                ItemNo = item.ID,
                Quantity = 1,
                Status = "Received",
                UserID = user.ID,
                OrderedImage = ImageString
            };

            if(ApiController.PlaceOrder(order))
            {
                LocalNotificationService.SendNotification("Order Recived");

                var route = $"{nameof(OrdersView)}";

                Shell.Current.GoToAsync(route);
            }
            else
            {
                DisplayAlert("Message", "Try Again After Some Time", "ok");
            }
        }
    }
}