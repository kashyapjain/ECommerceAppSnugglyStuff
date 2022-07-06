
using Plugin.FirebasePushNotification;
using SnugglyStuff.Models;
using SnugglyStuff.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnugglyStuff
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;
        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            Application.Current.Properties["Token"] = e.Token;   
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
