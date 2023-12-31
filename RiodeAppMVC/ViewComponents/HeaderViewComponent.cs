﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.DataAccess;

namespace RiodeAppMVC.ViewComponents
{
	public class HeaderViewComponent:ViewComponent
	{
		readonly RiodeDbContext _context;

        public HeaderViewComponent(RiodeDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value));
        }
    }
}

