using System;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.DataAccess;
using RiodeAppMVC.ExtensionServices.Interfaces;
using RiodeAppMVC.Models;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.SliderVMs;

namespace RiodeAppMVC.Services.Implements
{
    public class SliderService : ISliderService
    {
        readonly RiodeDbContext _context;
        readonly IFileService _fileService;

        public SliderService(RiodeDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public IQueryable<Slider> GetTable { get => _context.Set<Slider>(); }

        public async Task Create(CreateSliderVM sliderVm)
        {
            await _context.Sliders.AddAsync(new Slider
            {
                ImageUrl = await _fileService.UploadAsync(sliderVm.ImageFile, Path.Combine("imgs", "sliders")),
                Title = sliderVm.Title,
                Description = sliderVm.Description,
                ButtonText = sliderVm.ButtonText,
                isLeft = sliderVm.isLeft
            });
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            var entity = await GetById(id,true);
            _context.Sliders.Remove(entity);
            _fileService.Delete(entity.ImageUrl);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Slider>> GetAll(bool takeAll)
        {
            if(takeAll)
            {
              return await _context.Sliders.ToListAsync();
            }
            return await _context.Sliders.Where(s => s.isDeleted == false).ToListAsync();
        }

        public async Task<Slider> GetById(int? id, bool takeAll = false)
        {
            if (id == null || id <= 0) throw new NullReferenceException();
            Slider? entity;

            if(takeAll)
            {
                entity = await _context.Sliders.FindAsync(id);
            }
            else {
                entity = await _context.Sliders.SingleOrDefaultAsync(s => s.isDeleted == false && s.Id == id);
            }

            if (entity is null) throw new NullReferenceException();

            return entity;
        }

        public async Task SoftDelete(int? id)
        {
            var entity =await GetById(id,true);
            entity.isDeleted = !entity.isDeleted;
            await _context.SaveChangesAsync();
        }

        public async Task Update(int? id,UpdateSliderVM sliderVM)
        {
            var entity = await GetById(id);
            entity.Title = sliderVM.Title;
            entity.Description = sliderVM.Description;
            entity.ButtonText = sliderVM.ButtonText;
            entity.isLeft = sliderVM.isLeft;
            if(sliderVM.ImageFile != null)
            {
                _fileService.Delete(entity.ImageUrl);
                entity.ImageUrl = await _fileService.UploadAsync(sliderVM.ImageFile, Path.Combine("imgs", "sliders"));
            }
            await _context.SaveChangesAsync();
        }
    }
}

