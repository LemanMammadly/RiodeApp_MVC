using System;
using RiodeAppMVC.Models;

namespace RiodeAppMVC.ViewModels.BasketVMs
{
	public record BasketItemProductVM
	{
		public Product Product { get; set; }
		public int Count { get; set; }
	}
}


