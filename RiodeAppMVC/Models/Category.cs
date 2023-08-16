using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.Models
{
	public class Category:BaseEntity
	{
		[Required]
		public string Name { get; set; }
		public ICollection<ProductCategory>? ProductCategories { get; set; }
	}
}


