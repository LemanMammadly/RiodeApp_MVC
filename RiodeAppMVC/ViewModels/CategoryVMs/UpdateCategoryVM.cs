using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.ViewModels.CategoryVMs
{
	public record UpdateCategoryVM
	{
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}


