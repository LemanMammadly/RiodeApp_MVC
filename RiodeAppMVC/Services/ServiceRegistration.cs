using System;
using RiodeAppMVC.ExtensionServices.Implements;
using RiodeAppMVC.ExtensionServices.Interfaces;
using RiodeAppMVC.Services.Implements;
using RiodeAppMVC.Services.Interfaces;

namespace RiodeAppMVC.Services
{
	public static class ServiceRegistration
	{
		public static void AddService(this IServiceCollection services)
		{
			services.AddScoped<ISliderService, SliderService>();
			services.AddScoped<IFileService, FileService>();
			services.AddScoped<IProductService, ProductService>();
		}
	}
}

