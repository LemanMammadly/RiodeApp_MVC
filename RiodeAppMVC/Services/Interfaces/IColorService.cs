using System;
using RiodeAppMVC.Models;
using RiodeAppMVC.ViewModels.ColorVMs;

namespace RiodeAppMVC.Services.Interfaces
{
	public interface IColorService
	{
		IQueryable<Color> GetTable { get; }
		Task<ICollection<Color>> GetAll(bool takeAll);
		Task<Color> GetById(int? id, bool takeAll = false);
		Task Create(CreateColorVM colorVM);
		Task Update(int? id, UpdateColorVM colorVM);
		Task Delete(int? id);
	}
}

