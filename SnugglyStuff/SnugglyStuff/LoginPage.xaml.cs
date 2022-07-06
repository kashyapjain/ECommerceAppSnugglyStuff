using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SnugglyStuff.Models;
using SnugglyStuff.Controller;

namespace SnugglyStuff
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login(object sender, EventArgs e)
        {
            User user = new User
            {
                Email = Email.Text,
                Password = Password.Text
            };

            if (ApiController.LoginOrRegistration(user,UriModel.LoginUrl).Result)
            {
                DisplayAlert("Message", "Logedin Successfully", "ok");

                Shell.Current.Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Alert","Unauthorised","ok");
            }
        }

        private void Register(object sender, EventArgs e)
        {
            var route = $"{nameof(RegistrationPage)}";

            Shell.Current.GoToAsync(route);
        }
    }
}