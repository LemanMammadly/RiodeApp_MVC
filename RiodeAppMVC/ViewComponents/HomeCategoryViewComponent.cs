using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.DataAccess;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.CategoryVMs;

namespace RiodeAppMVC.ViewComponents
{
	public class HomeCategoryViewComponent:ViewComponent
	{
        readonly RiodeDbContext _context;
        readonly IProductService _productService;
        readonly ICategoryService _categoryService;


        public HomeCategoryViewComponent(IProductService productService, RiodeDbContext context, ICategoryService categoryService)
        {
            _productService = productService;
            _context = context;
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryData = await _categoryService.GetTable.Take(4)
                .Include(cs => cs.ProductCategories)
                .ThenInclude(pc => pc.Product)
                .ToListAsync();

            List<HomeCategoryVM> categoryVMs = categoryData.Select(category => new HomeCategoryVM
            {
                CategoryName = category.Name,
                Count = category.ProductCategories.Count,
                ProductCount = category.ProductCategories.Sum(pc => pc.Product != null ? 1 : 0),
                ImageUrl = category.ProductCategories.FirstOrDefault()?.Product?.MainImage,
                Products = category.ProductCategories.Select(pc => pc.Product)
            }).ToList();

            return View(categoryVMs);
        }
    }
}

