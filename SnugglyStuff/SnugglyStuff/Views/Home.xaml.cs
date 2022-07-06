using MvvmHelpers;
using SnugglyStuff.Controller;
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
    public partial class Home : ContentPage
    {
        private IEnumerable<Item> _Items;
        private IEnumerable<Item> Items
        {
            get
            {
                return _Items;
            }

            set
            {
                if (Equals(_Items, value))
                    return;

                _Items = value;

                OnPropertyChanged("Items");
            }
        }

        public Home()
        {
            InitializeComponent();

            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            try
            {
                var ItemsList = ApiController.GetData();

                Items = ItemsList;

                ItemsListView.ItemsSource = Items;
            }
            catch (Exception e)
            {

            }
            base.OnAppearing();
        }

        public async Task UploadFile(Item item)
        {
            try
            {
                var photo = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Png,
                    PickerTitle = "Select an Image"
                });

                var newFile = Path.Combine(FileSystem.AppDataDirectory, item.ID.ToString() + "Second.png");

                using (var stream = photo.OpenReadAsync().Result)
                {
                    using (var newStream = File.OpenWrite(newFile))
                    {
                        await stream.CopyToAsync(newStream);
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {

            }
        }

        private async void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var ItemSelected = ((ListView)sender).SelectedItem as Item;

            if(ItemSelected == null)
            {
                return;
            }

            ItemCoustomizationView.item = ItemSelected;

            //Alert

            await DisplayAlert("Message","Upload logo to Continue","ok");

            //upload file

            await UploadFile(ItemSelected);

            //Navigation

            var route = $"{nameof(ItemCoustomizationView)}";

            Shell.Current.GoToAsync(route);
        }

        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}