using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.ColorVMs;

namespace RiodeAppMVC.Areas.Manage.Controllers;

[Area("Manage")]
public class ColorController : Controller
{
    readonly IColorService _colorService;

    public ColorController(IColorService colorService)
    {
        _colorService = colorService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _colorService.GetAll(true));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateColorVM colorVM)
    {
        if (!ModelState.IsValid) return View();
        if (await _colorService.GetTable.AnyAsync(c => c.Code == colorVM.Code))
        {
            ViewBag.ColorError = "This color name is already exist.";
            return View();
        }
        await _colorService.Create(colorVM);
        return RedirectToAction(nameof(Index));
    }

    public async  Task<IActionResult> Update(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity =await _colorService.GetById(id);
        if (entity is null) return BadRequest();

        UpdateColorGetVM updateColor = new UpdateColorGetVM()
        {
            Code = entity.Code
        };
        return View(updateColor);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int? id,UpdateColorGetVM getVM)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _colorService.GetById(id);
        if (entity is null) return BadRequest();

        UpdateColorVM colorVM = new UpdateColorVM()
        {
            Code = getVM.Code
        };

        if(await _colorService.GetTable.AnyAsync(c=>c.Code==getVM.Code))
        {
            ViewBag.ColorError = "This color name is already exist.";
            return View();
        }

        await _colorService.Update(id, colorVM);
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        try
        {
            await _colorService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "This color have products.Cant delete this color!";
            return RedirectToAction(nameof(Index));
        }
    }
}

