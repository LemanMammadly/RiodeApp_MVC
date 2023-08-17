using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RiodeAppMVC.DataAccess;
using RiodeAppMVC.Services.Interfaces;
using RiodeAppMVC.ViewModels.BasketVMs;
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
        ViewBag.ProductCount = await _productService.GetTable.CountAsync();
        return View(vm);
    }

    public async Task<IActionResult> AddBasket(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        if (!await _productService.GetTable.AllAsync(p => p.Id == id)) return NotFound();
        var basket = HttpContext.Request.Cookies["basket"];
        List<BasketItemVM> items = basket == null ? new List<BasketItemVM>() : JsonConvert.DeserializeObject<List<BasketItemVM>>(basket);
        var item = items.SingleOrDefault(i => i.Id == id);
        if(item==null)
        {
            item = new BasketItemVM
            {
                Id = (int)id,
                Count = 1
            };
            items.Add(item);
        }
        else {
            item.Count++;
        }
        HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(items));
        return Ok();
    }

    public async Task<IActionResult> GetBasket()
    {
        var basket = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
        List<BasketItemProductVM> basketVM = new List<BasketItemProductVM>();
        foreach (var item in basket)
        {
            basketVM.Add(new BasketItemProductVM
            {
                Count = item.Count,
                Product = await _productService.GetById(item.Id)
            });
        }
        return PartialView("_BasketPartial", basketVM);
    }
}

