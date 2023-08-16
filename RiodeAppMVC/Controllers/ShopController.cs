using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.ShopVMs;

namespace RiodeAppMVC.Controllers
{
    public class ShopController : Controller
    {
        readonly IProductService _productService;


        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            ShopVM vm = new ShopVM()
            {
                Products = await _productService.GetTable.Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category).Take(10).ToListAsync()
            };
            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var entity =await _productService.GetTable.Include(pc=>pc.ProductImages)
                .Include(pc=>pc.ProductCategories)
                .ThenInclude(pc=>pc.Category)
                .SingleOrDefaultAsync(ct=>ct.Id==id);
            return View(entity);
        }
    }
}

