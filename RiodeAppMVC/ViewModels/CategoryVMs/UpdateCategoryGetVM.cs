using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.ViewModels.CategoryVMs
{
	public record UpdateCategoryGetVM
	{
        [Required]
        public string Name { get; set; }
    }
}

