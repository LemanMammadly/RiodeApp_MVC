using System;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.DataAccess;
using RiodeAppMVC.Models;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.ColorVMs;

namespace RiodeAppMVC.Services.Implements;

public class ColorService : IColorService
{
    readonly RiodeDbContext _context;

    public ColorService(RiodeDbContext context)
    {
        _context = context;
    }

    public IQueryable<Color> GetTable { get => _context.Set<Color>(); }

    public async Task Create(CreateColorVM colorVM)
    {
        if (await _context.Colors.AnyAsync(c => c.Code == colorVM.Code))
        {
            throw new Exception();
        }
        Color color = new Color()
        {
            Code = colorVM.Code
        };
        await _context.Colors.AddAsync(color);
        await _context.SaveChangesAsync();
    }

    public Task Delete(int? id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Color>> GetAll(bool takeAll)
    {
        if(takeAll)
        {
          return await _context.Colors.ToListAsync();
        }
        else {
            return await _context.Colors.Where(c => c.isDeleted == false).ToListAsync();
        }
    }

    public async Task<Color> GetById(int? id, bool takeAll = false)
    {
        if (id == null || id <= 0) throw new Exception();

        Color? entity;

        if(takeAll)
        {
            entity = await _context.Colors.FindAsync(id);
        }
        else {
            entity = await _context.Colors.SingleOrDefaultAsync(c => c.isDeleted == false && c.Id == id);
        }

        if (entity is null) throw new Exception();
        return entity;
    }

    public async Task Update(int? id, UpdateColorVM colorVM)
    {
        var entity =await GetById(id);
        if(await _context.Colors.AnyAsync(c => c.Code == colorVM.Code))
        {
            throw new Exception();
        }
        else {
            entity.Code = colorVM.Code;
            await _context.SaveChangesAsync();
        }
    }
}

