using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.ViewModels.ColorVMs
{
	public record CreateColorVM
	{
        [Required]
        public string Code { get; set; }
    }
}

