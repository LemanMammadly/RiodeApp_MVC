using System;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.Models;

namespace RiodeAppMVC.DataAccess
{
	public class RiodeDbContext:DbContext
	{
		public RiodeDbContext(DbContextOptions options) : base(options) { }
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
	}
}


