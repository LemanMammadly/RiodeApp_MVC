using System;
using RiodeAppMVC.Models;

namespace RiodeAppMVC.ViewModels.CategoryVMs
{
	public class HomeCategoryVM
	{
        public string CategoryName { get; set; }
        public int Count { get; set; }
        public int ProductCount { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}

