using SnugglyStuff.Controller;
using SnugglyStuff.Models;
using SnugglyStuff.Service;
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
    public partial class OrdersView : ContentPage
    {
        static Order order;
        static string UserID = Application.Current.Properties["ID"].ToString();
        private IEnumerable<Order> _Orders;
        private IEnumerable<Order> Orders
        {
            get
            {
                return _Orders;
            }

            set
            {
                if (Equals(_Orders, value))
                    return;

                _Orders = value;

                OnPropertyChanged("Orders");
            }
        }
        public OrdersView()
        {
            InitializeComponent();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            try
            {
                var OrdersList = ApiController.GetOrders(UserID);

                Orders = OrdersList;

                OrdersListView.ItemsSource = Orders.Reverse();
            }
            catch (Exception e)
            {

            }
            base.OnAppearing();
        }

        private void Reorder(object sender, EventArgs e)
        {
            var ID = ((Button)sender).CommandParameter.ToString();
            
            if (ApiController.Reorder(ID))
            {
                LocalNotificationService.SendNotification("Order Recived");

                OnAppearing();
            }
            else
            {
                DisplayAlert("Message", "Try Again After Some Time", "ok");
            }
        }
    }
}