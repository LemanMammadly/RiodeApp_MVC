using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.DataAccess;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.HomeVMs;

namespace RiodeAppMVC.Controllers;

public class HomeController : Controller
{
    readonly RiodeDbContext _context;
    readonly ISliderService _sliderService;
    readonly IProductService _productService;
    readonly ICategoryService _categoryService;

    public HomeController(RiodeDbContext context, ISliderService sliderService, IProductService productService, ICategoryService categoryService)
    {
        _context = context;
        _sliderService = sliderService;
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        HomeVM vm = new HomeVM
        {
            Sliders = await _sliderService.GetAll(false),
            Products = await _productService.GetAll(false),
            Categories = await _categoryService.GetTable.Take(4).ToListAsync()
        };
        return View(vm);
    }
}

