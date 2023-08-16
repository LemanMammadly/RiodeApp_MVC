using System;
using RiodeAppMVC.Models;
using RiodeAppMVC.ViewModels.CategoryVMs;

namespace RiodeAppMVC.Services.Interfaces;

public interface ICategoryService
{
	IQueryable<Category> GetTable { get; }
	Task<ICollection<Category>> GetAll(bool takeAll);
	Task<Category> GetById(int? id, bool takeAll = false);
	Task Create(CreateCategoryVM createVm);
	Task Update(int? id, UpdateCategoryVM updateVM);
	Task<bool> IsExist(int id);
	Task<bool> IsAllExist(List<int> ids);
	Task Delete(int? id);
	Task SoftDelete(int? id);
}

