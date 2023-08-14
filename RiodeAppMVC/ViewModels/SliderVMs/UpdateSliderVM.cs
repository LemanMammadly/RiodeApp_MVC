using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.ViewModels.SliderVMs
{
	public record UpdateSliderVM
	{
        public int? Id { get; set; }
        [Required, MaxLength(30)]
        public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Description { get; set; }
        public IFormFile? ImageFile { get; set; }
        [Required, MaxLength(30)]
        public string ButtonText { get; set; }
        [Required]
        public bool isLeft { get; set; }
    }
}


