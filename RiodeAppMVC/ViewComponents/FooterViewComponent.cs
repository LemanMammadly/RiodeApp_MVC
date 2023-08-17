﻿using System;
using Microsoft.AspNetCore.Mvc;
using RiodeAppMVC.DataAccess;

namespace RiodeAppMVC.ViewComponents
{
	public class FooterViewComponent:ViewComponent
	{
        readonly RiodeDbContext _context;

        public FooterViewComponent(RiodeDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

