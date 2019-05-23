using CMS.DocumentEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class HomeViewModel
    {
        public List<Product> Products;
        public HomeViewModel()
        {
            Products = new List<Product>();
        }
    }
}