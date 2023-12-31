﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RiodeAppMVC.DataAccess;
using RiodeAppMVC.ViewModels.BasketVMs;

namespace RiodeAppMVC.ViewComponents;

public class BasketViewComponent:ViewComponent
{
	readonly RiodeDbContext _context;
    readonly IHttpContextAccessor _contextAccessor;

    public BasketViewComponent(RiodeDbContext context, IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _contextAccessor = contextAccessor;
    }

    public async Task<IViewComponentResult> InvokeAsync()
	{
        List<BasketItemVM> items = new();
        if (_contextAccessor.HttpContext.Request.Cookies["basket"] != null)
        {
            items = JsonConvert.DeserializeObject<List<BasketItemVM>>(_contextAccessor.HttpContext.Request.Cookies["basket"]);
        }

        List<BasketItemProductVM> products = new List<BasketItemProductVM>();
        float sum = 0;
        foreach (var item in items)
        {
            var prodItem = new BasketItemProductVM()
            {
                Product = await _context.Products.Include(p => p.ProductImages).SingleOrDefaultAsync(p => p.Id == item.Id),
                Count = item.Count
            };

            sum += (float)((prodItem.Product.Price * (100 - prodItem.Product.Discount) / 100) * item.Count);

            products.Add(prodItem);
        }
        ViewBag.SubTotal = sum;
        return View(products);
	}
}

