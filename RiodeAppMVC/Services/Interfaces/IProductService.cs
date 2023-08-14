using System;
using RiodeAppMVC.Models;
using RiodeAppMVC.ViewModels.ProductVMs;

namespace RiodeAppMVC.Services.Interfaces
{
	public interface IProductService
	{
		IQueryable<Product> GetTable { get; }
		Task<ICollection<Product>> GetAll(bool TakeAll);
		Task<Product> GetById(int? id, bool TakeAll = false);
		Task Create(CreateProductVM productVM);
		Task Update(int? id,UpdateProductVM productVM);
		Task DeleteImage(int? id);
		Task Delete(int? id);
		Task SoftDelete(int? id);
	}
}

