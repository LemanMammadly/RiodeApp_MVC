using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.Models
{
	public class Color:BaseEntity
	{
		[Required]
		public string Code { get; set; }
		public ICollection<ProductColor>? ProductColors { get; set; }
	}
}



