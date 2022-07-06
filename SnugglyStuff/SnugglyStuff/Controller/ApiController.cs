using Newtonsoft.Json;
using SnugglyStuff.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SnugglyStuff.Controller
{
    class ApiController
    {
        static HttpClient client = new HttpClient();

        public static bool PlaceOrder(Order order)
        {
            try
            {
                var json = JsonConvert.SerializeObject(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.PostAsync(UriModel.PlaceorderUrl, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<byte[]> GetImageBytes(string ImageUrl)
        {
            try
            {
                using (var response = client.GetAsync(ImageUrl).Result)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return await response.Content.ReadAsByteArrayAsync();
                    }
                    else
                    {
                        //Url is Invalid
                        return null;
                    }
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static List<Category> GetCategories()
        {
            try
            {
                var response = client.GetAsync(UriModel.CategoriesUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var categories = JsonConvert.DeserializeObject<List<Category>>(json);

                    foreach(var e in categories)
                    {
                        e.Image = UriModel.ImageUrl + "Categories/" + e.Image;
                    }

                    return categories;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static List<Sliders> GetSliders()
        {
            try
            {
                var response = client.GetAsync(UriModel.SlidersUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var sliders = JsonConvert.DeserializeObject<List<Sliders>>(json);

                    foreach (var e in sliders)
                    {
                        e.Image = UriModel.ImageUrl + "Sliders/" + e.Image;
                    }

                    return sliders;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static User GetUserdetails(string id)
        {
            try
            {
                client = new HttpClient();

                client.DefaultRequestHeaders.Add("Authorization"
                    , "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(id)));

                var response = client.GetAsync(UriModel.UserDetailsUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var user = JsonConvert.DeserializeObject<User>(json);

                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static async Task<bool> LoginOrRegistration(User user,string url)
        {
            try
            {
                user.Token = Application.Current.Properties["Token"].ToString();

                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    var id = response.Content.ReadAsStringAsync().Result;

                    Application.Current.Properties["ID"] = id;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public static List<Item> GetItems(string CategoryID)
        {
            try
            {
                var response = client.GetAsync(UriModel.ItemsUri + "/" + CategoryID).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var Items = JsonConvert.DeserializeObject<List<Item>>(json);

                    foreach (var i in Items)
                    {
                        i.ImageSrc = UriModel.ImageUrl + i.ImageSrc;
                        i.Price = "$ " + i.Price;
                    }

                    return Items;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Order> GetOrders(string UserID)
        {
            try
            {
                var response = client.GetAsync(UriModel.OrdersUrl + "/" + UserID).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var Orders = JsonConvert.DeserializeObject<List<Order>>(json);

                    foreach (var i in Orders)
                    {
                        i.OrderedImage = UriModel.ImageUrl + "Orders/" + i.OrderedImage;
                        i.DateAndTime = Convert.ToDateTime(i.DateAndTime).ToShortDateString();
                    }

                    return Orders;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool Reorder(string OrderID)
        {
            try
            {
                var response = client.GetAsync(UriModel.ReorderUrl + "/" + OrderID).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static List<Item> GetData()
        {
            try
            {
                var response = client.GetAsync(UriModel.ItemsUri).Result;

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var Items = JsonConvert.DeserializeObject<List<Item>>(json);

                    foreach (var i in Items)
                    {
                        i.ImageSrc = UriModel.ImageUrl + i.ImageSrc;
                        i.Price = "$ " + i.Price;
                    }

                    return Items;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }
    }
}
