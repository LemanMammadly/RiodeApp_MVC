using System;
namespace RiodeAppMVC.ViewModels.BasketVMs
{
	public record BasketItemVM
	{
		public int Id { get; set; }
		public int Count { get; set; }
	}
}