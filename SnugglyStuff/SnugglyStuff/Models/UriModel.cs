using System;
using System.Collections.Generic;
using System.Text;

namespace SnugglyStuff.Models
{
    class UriModel
    {
        //Main Url
        public static string MainUrl
        {
            get
            {
                return "http://192.168.1.8/";
            }
        }

        //Web Api Urls
        public static string BaseUrl
        {
            get
            {
                return MainUrl + "SnugglyStuffWebApp/api/data/";
            }
        }
        public static string ReorderUrl
        {
            get
            {
                return BaseUrl + "Reorder";
            }
        }
        public static string OrdersUrl
        {
            get
            {
                return BaseUrl + "GetOrders";
            }
        }
        public static string UserDetailsUrl
        {
            get
            {
                return BaseUrl + "GetUserDetails";
            }
        }
        public static string RegistrationUrl
        {
            get
            {
                return BaseUrl + "register";
            }
        }
        public static string LoginUrl
        {
            get
            {
                return BaseUrl + "login";
            }
        }
        public static string ItemsUri
        {
            get
            {
                return BaseUrl + "GetItems";
            }
        }
        public static string SlidersUri
        {
            get
            {
                return BaseUrl + "GetSliders";
            }
        }
        public static string CategoriesUri
        {
            get
            {
                return BaseUrl + "GetCategories";
            }
        }
        //MVC Urls
        public static string MVCUrl
        {
            get
            {
                return MainUrl + "SnugglyStuffMVC/";
            }
        }
        public static string ImageUrl
        {
            get
            {
                return MVCUrl + "Images/";
            }
        }

        public static string PlaceorderUrl
        {
            get
            {
                return MVCUrl + "WebApi/PlaceOrder";
            }
        }
    }
}
