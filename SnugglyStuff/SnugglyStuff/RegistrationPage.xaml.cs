using SnugglyStuff.Controller;
using SnugglyStuff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SnugglyStuff
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void Register(object sender, EventArgs e)
        {
            User user = new User
            {
                Email = Email.Text,
                Address = Address.Text,
                PhoneNo = PhoneNo.Text,
                Password = Password.Text
            };

            if(ApiController.LoginOrRegistration(user,UriModel.RegistrationUrl).Result)
            {
                DisplayAlert("Message", "Registered Successfully", "ok");

                Shell.Current.Navigation.PopAsync();
                Shell.Current.Navigation.PopAsync();

                ApiController.LoginOrRegistration(user, UriModel.LoginUrl);
            }
        }
    }
}