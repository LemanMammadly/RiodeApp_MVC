using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.CategoryVMs;

namespace RiodeAppMVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,Editor")]
    public class CategoryController : Controller
    {
        readonly ICategoryService _catService;
        readonly IProductService _prodService;

        public CategoryController(ICategoryService catService, IProductService prodService)
        {
            _catService = catService;
            _prodService = prodService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catService.GetAll(true));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View();
            await _catService.Create(categoryVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var entity =await _catService.GetById(id);
            if (entity == null) return BadRequest();

            UpdateCategoryGetVM getVM = new UpdateCategoryGetVM()
            {
                Name=entity.Name
            };
            return View(getVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateCategoryGetVM getVM)
        {
            if (id == null || id <= 0) return BadRequest();
            var entity = await _catService.GetById(id);
            if (entity == null) return BadRequest();


            UpdateCategoryVM categoryVM = new UpdateCategoryVM()
            {
                Name = getVM.Name
            };

            if(await _catService.GetTable.AnyAsync(c=>c.Name==getVM.Name))
            {
                ViewBag.CategoryError = "This category name is already exist.";
                return View();
            }

            await _catService.Update(id, categoryVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            try
            {
                await _catService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "This category have products.Cant delete this category!";
                return RedirectToAction(nameof(Index)); 
            }
        }

        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            try
            {
                await _catService.SoftDelete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "This category have products.Cant delete this category!";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}

