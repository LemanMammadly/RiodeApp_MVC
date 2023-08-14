﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.ViewModels.SliderVMs
{
	public record CreateSliderVM
	{
        [Required, MaxLength(30)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
        [Required, MaxLength(30)]
        public string ButtonText { get; set; }
        [Required]
        public bool isLeft { get; set; }
    }
}

