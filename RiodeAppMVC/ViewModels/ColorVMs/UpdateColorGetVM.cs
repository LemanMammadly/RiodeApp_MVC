using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.ViewModels.ColorVMs;

	public record UpdateColorGetVM
	{
    public int Id { get; set; }
    [Required]
    public string Code { get; set; }
}

