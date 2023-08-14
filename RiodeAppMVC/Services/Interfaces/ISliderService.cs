using System;
using RiodeAppMVC.Models;
using RiodeAppMVC.ViewModels.SliderVMs;

namespace RiodeAppMVC.Services.Interfaces;

public interface ISliderService
{
	IQueryable<Slider> GetTable { get; }
	Task Create(CreateSliderVM sliderVm);
	Task<ICollection<Slider>> GetAll(bool takeAll);
	Task <Slider> GetById(int? id,bool takeAll=false);
	Task Update(int? id,UpdateSliderVM sliderVM);
	Task Delete(int? id);
	Task SoftDelete(int? id);
}

