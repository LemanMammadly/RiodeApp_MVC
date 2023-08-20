using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RiodeAppMVC.Models;
using RiodeAppMVC.ViewModels.UserVMs;


namespace RiodeAppMVC.Areas.Manage.Controllers;
[Area("Manage")]
[Authorize(Roles = "Admin")]

public class UserController : Controller
{
    readonly UserManager<AppUser> _userManager;
    readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        List<UserAndRoleVM> vm = new List<UserAndRoleVM>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            //if (roles.Contains("Admin"))
            //    continue;
            vm.Add(new UserAndRoleVM
            {
                Fullname = user.Fullname,
                Name = user.UserName,
                Role = roles.FirstOrDefault()
            });
        }
        ViewBag.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> ChangeRole(string username, string role)
    {
        var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);
        var user = await _userManager.FindByNameAsync(username);
        if (currentUser == null || user == null) return BadRequest();
        var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
        var userRoles = await _userManager.GetRolesAsync(user);

        if (userRoles.FirstOrDefault() == "Admin")
        {
            return BadRequest();
        }
        else
        {
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRoleAsync(user, role);
        }
        return Ok();
    }
}
