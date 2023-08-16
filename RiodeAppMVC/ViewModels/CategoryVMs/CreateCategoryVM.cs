using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.ViewModels.CategoryVMs;

public class CreateCategoryVM
{
    [Required]
    public string Name { get; set; }
}

