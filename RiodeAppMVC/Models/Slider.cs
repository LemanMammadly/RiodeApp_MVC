using System;
using System.ComponentModel.DataAnnotations;

namespace RiodeAppMVC.Models
{
	public class Slider:BaseEntity
	{
		[Required,MaxLength(30)]
		public string Title { get; set; }
        [Required, MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required, MaxLength(30)]
        public string ButtonText { get; set; }
        [Required]
        public bool isLeft { get; set; }
    }
}

