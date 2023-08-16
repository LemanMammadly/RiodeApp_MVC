using System;
using RiodeAppMVC.Models;

namespace RiodeAppMVC.ViewModels.HomeVMs
{
	public class HomeVM
	{
		public ICollection<Slider> Sliders { get; set; }
		public ICollection<Product> Products { get; set; }
		public ICollection<Category> Categories { get; set; }
	}
}




