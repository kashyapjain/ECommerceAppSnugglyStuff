using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnugglyStuffWebApp.Models
{
    public class FirebaseTokenModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public string Token { get; set; }
    }
}