using System;
using System.Collections.Generic;
using System.Text;

namespace SnugglyStuff.Models
{
    internal class Order
    {
        public int ID { get; set; }
        public Nullable<int> ItemNo { get; set; }
        public Nullable<int> UserID { get; set; }
        public string DateAndTime { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Status { get; set; }
        public string OrderedImage { get; set; }
    }
}
