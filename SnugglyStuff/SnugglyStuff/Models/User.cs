using System;
using System.Collections.Generic;
using System.Text;

namespace SnugglyStuff.Models
{
    class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
