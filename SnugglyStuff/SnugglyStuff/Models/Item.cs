using System;
using System.Collections.Generic;
using System.Text;

namespace SnugglyStuff.Models
{
    public class Item
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Price { get; set; }
        public string ImageSrc { get; set; }
        public Nullable<int> CategoryID { get; set; }
    }
}
