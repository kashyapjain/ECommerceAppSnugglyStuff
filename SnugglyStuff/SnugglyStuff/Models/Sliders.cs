using System;
using System.Collections.Generic;
using System.Text;

namespace SnugglyStuff.Models
{
    class Sliders
    {
        public int ID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string Image { get; set; }

        public virtual Category Category { get; set; }
    }
}
