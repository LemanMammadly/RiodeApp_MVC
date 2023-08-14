﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.Extensions;
using RiodeAppMVC.Models;
using RiodeAppMVC.Services.Implements;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.ProductVMs;

namespace RiodeAppMVC.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAll(true));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if(productVM.MainImageFile != null)
            {
                if(!productVM.MainImageFile.IsTypeValid("image"))
                {
                    ModelState.AddModelError("MainImageFile", "Image type is not valid");
                }

                if(!productVM.MainImageFile.IsSizeValid(2))
                {
                    ModelState.AddModelError("MainImageFile", "Image size must be smaller or equal 2 mb");
                }
            }

            if(productVM.ImageFiles != null)
            {
                foreach (var item in productVM.ImageFiles)
                {
                    if(!item.IsTypeValid("image"))
                    {
                        ModelState.AddModelError("Item", "Image type is not valid");
                    }
                    if(!item.IsSizeValid(2))
                    {
                        ModelState.AddModelError("Item", "Image size must be smaller or equal 2 mb");
                    }
                }
            }

            if (!ModelState.IsValid) return View();

            await _productService.Create(productVM);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var entity = await _productService.GetTable.Include(p => p.ProductImages).SingleOrDefaultAsync(p => p.Id == id);
            if (entity == null) return BadRequest();
            UpdateProductGetVM updateProduct = new UpdateProductGetVM()
            {
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Discount = entity.Discount,
                Rating = entity.Rating,
                StockCount = entity.StockCount,
                MainImage = entity.MainImage,
                ProductImages=entity.ProductImages
            };
            return View(updateProduct);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateProductGetVM productGetVM)
        {
            if (id == null || id <= 0) return BadRequest();
            var entity = await _productService.GetTable.Include(p => p.ProductImages).SingleOrDefaultAsync(p => p.Id == id);
            if (entity == null) return BadRequest();


            UpdateProductVM updateProduct = new UpdateProductVM()
            {
                Name = productGetVM.Name,
                Description = productGetVM.Description,
                Price = productGetVM.Price,
                Discount = productGetVM.Discount,
                Rating = productGetVM.Rating,
                StockCount = productGetVM.StockCount,
                MainImage=productGetVM.MainImageFile,
                ProductImagesFiles=productGetVM.ProductImageFiles
            };
            await _productService.Update(id, updateProduct);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            await _productService.DeleteImage(id);
            return Ok();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var entity = await _productService.GetTable.Include(p => p.ProductImages).SingleOrDefaultAsync(p => p.Id == id);
            if (entity == null) return BadRequest();
            await _productService.Delete(entity.Id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            await _productService.SoftDelete(id);
            TempData["IsDeleted"] = true;
            return RedirectToAction(nameof(Index));
        }

    }
}
