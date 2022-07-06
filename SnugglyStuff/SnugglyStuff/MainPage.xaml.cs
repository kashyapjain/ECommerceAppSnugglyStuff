using SnugglyStuff.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ItemsView = SnugglyStuff.Views.ItemsView;

namespace SnugglyStuff
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }

            Routing.RegisterRoute(nameof(ItemCoustomizationView),
                typeof(ItemCoustomizationView));

            Routing.RegisterRoute(nameof(PlaceOrderView), typeof(PlaceOrderView));
            
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            
            Routing.RegisterRoute(nameof(Home), typeof(Home));

            Routing.RegisterRoute(nameof(ItemsView), typeof(ItemsView));

            Routing.RegisterRoute(nameof(ItemView), typeof(ItemView));

            Routing.RegisterRoute(nameof(BitmapScatterViewPage), typeof(BitmapScatterViewPage));

            Routing.RegisterRoute(nameof(OrdersView), typeof(OrdersView));
        }

       
    }
}
