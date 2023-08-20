using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiodeAppMVC.DataAccess;
using RiodeAppMVC.Extensions;
using RiodeAppMVC.Models;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.SliderVMs;

namespace RiodeAppMVC.Areas.Manage.Controllers;

[Area("Manage")]
[Authorize(Roles = "Admin,Editor")]
public class SliderController : Controller
{
    readonly ISliderService _sliderService;

    public SliderController(RiodeDbContext context, ISliderService sliderService)
    {
        _sliderService = sliderService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _sliderService.GetAll(true));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSliderVM vm)
    {
        if (vm.ImageFile != null)
        {
            if (!vm.ImageFile.IsTypeValid("image"))
            {
                ModelState.AddModelError("ImageFile", "Wrong file type");
            }
            if (!vm.ImageFile.IsSizeValid(2))
            {
                ModelState.AddModelError("ImageFile", "File max size 2mb");
            }
            if (!ModelState.IsValid) return View();
            await _sliderService.Create(vm);
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _sliderService.GetById(id);
        if (entity == null) return BadRequest();
        UpdateSliderGetVM vm = new UpdateSliderGetVM
        {
            Title = entity.Title,
            Description = entity.Description,
            isLeft = entity.isLeft,
            ButtonText = entity.ButtonText,
            Image=entity.ImageUrl
        };
        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(int? id,UpdateSliderGetVM vm)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _sliderService.GetById(id);
        if (entity == null) return BadRequest();
        UpdateSliderVM updateSlider = new UpdateSliderVM
        {
            Title = vm.Title,
            Description = vm.Description,
            isLeft = vm.isLeft,
            ButtonText = vm.ButtonText,
            ImageFile = vm.ImageFile
        };
        await _sliderService.Update(id, updateSlider);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        await _sliderService.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ChangeStatus(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        await _sliderService.SoftDelete(id);
        TempData["IsDeleted"] = true;
        return RedirectToAction(nameof(Index));
    }
}

