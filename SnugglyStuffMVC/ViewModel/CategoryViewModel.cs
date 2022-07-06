using SnugglyStuffMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnugglyStuffMVC.ViewModel
{
    public class CategoryViewModel
    {
        public static List<Category> categories
        {
            get
            {
                return new SnugglyStuffEntities().Categories.ToList();
            }
            set
            { }
        }
    }
}