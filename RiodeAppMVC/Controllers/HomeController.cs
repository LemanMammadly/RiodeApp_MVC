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

    public HomeController(RiodeDbContext context, ISliderService sliderService, IProductService productService)
    {
        _context = context;
        _sliderService = sliderService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        HomeVM vm = new HomeVM
        {
            Sliders = await _sliderService.GetAll(false),
            Products = await _productService.GetAll(false)
        };
        return View(vm);
    }
}

