
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
        }

        protected async override void OnStart()
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
