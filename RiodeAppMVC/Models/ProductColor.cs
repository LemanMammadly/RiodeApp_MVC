using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.Models
{
	public class ProductColor:BaseEntity
	{
		[Required]
		public int ColorId { get; set; }
		public Color? Color { get; set; }
        [Required]
        public int ProductId { get; set; }
		public Product? Product { get; set; }
	}
}


