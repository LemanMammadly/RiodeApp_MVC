using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RiodeAppMVC.Models
{
	public class AppUser:IdentityUser
	{
        [Required]
        public string Fullname { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

