using System;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.DataAccess;
using RiodeAppMVC.Models;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.CategoryVMs;

namespace RiodeAppMVC.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        readonly RiodeDbContext _context;

        public CategoryService(RiodeDbContext context)
        {
            _context = context;
        }

        public IQueryable<Category> GetTable { get => _context.Set<Category>(); }

        public async Task Create(CreateCategoryVM createVm)
        {
            if(await _context.Categories.AnyAsync(c=>c.Name==createVm.Name))
            {
                throw new Exception();
            }
            Category category = new Category()
            {
                Name = createVm.Name
            };
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {

            var entity =await GetById(id);
            var catEntity = await _context.Products.Where(p=>p.ProductCategories.Any(pc=>pc.CategoryId==id)).ToListAsync();
            if (catEntity.Count <= 0)
            {
              _context.Categories.Remove(entity);
              await _context.SaveChangesAsync();
            }
            else {
                throw new Exception();
            }

        }

        public async Task<ICollection<Category>> GetAll(bool takeAll)
        {
            if(takeAll)
            {
                return await _context.Categories.ToListAsync();
            }
            return await _context.Categories.Where(c => c.isDeleted == false).ToListAsync();
        }

        public async Task<Category> GetById(int? id, bool takeAll = false)
        {
            if (id == null || id <= 0) throw new ArgumentNullException();
            
            Category? entity;

            if(takeAll)
            {
                entity = await _context.Categories.FindAsync(id);
            }
            else {
                entity = await _context.Categories.SingleOrDefaultAsync(c => c.isDeleted == false && c.Id == id);
            }

            if (entity is null) throw new ArgumentNullException();

            return entity;
        }

        public async Task<bool> IsAllExist(List<int> ids)
        {
            foreach (var id in ids)
            {
                if(!await IsExist(id))
                    return false;
            }
            return true;
        }

        public async Task<bool> IsExist(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task SoftDelete(int? id)
        {
            var entity = await GetById(id,true);
            var catEntity = await _context.Products.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == id)).ToListAsync();
            if (catEntity.Count <= 0)
            {
                entity.isDeleted = !entity.isDeleted;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception();
            }
            await _context.SaveChangesAsync();
        }

        public async Task Update(int? id, UpdateCategoryVM updateVM)
        {
            var entity = await GetById(id);

            if (await _context.Categories.AnyAsync(c => c.Name == updateVM.Name))
            {
                throw new Exception();
            }
            else
            {
                entity.Name = updateVM.Name;
            }
            await _context.SaveChangesAsync();
        }
    }
}

