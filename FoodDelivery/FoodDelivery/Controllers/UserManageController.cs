using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.BLL.Interfaces;
using FoodDelivery.DAL.Models;
using FoodDelivery.DAL.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FoodDelivery.Controllers
{
    public class UserManageController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManageController(IUserService userService, UserManager<ApplicationUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string role="")
        {
            var users = await _userService.GetApplicatinoUsersByRole(role);

            return View(users);
        }

        public async Task<IActionResult> BlockUser(string id)
        {
            var user = _userService.GetApplicationUser(id);
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return View("Index", _userService.GetApplicationUsers());
        }

        public async Task<IActionResult> UnblockUser(string id)
        {
            var user = _userService.GetApplicationUser(id);
            await _userManager.SetLockoutEnabledAsync(user, false);
            return View("Index", _userService.GetApplicationUsers());
        }

        public IActionResult DeleteUser(string id)
        {
            _userService.Delete(id);
            return View("Index", _userService.GetApplicationUsers());
        }

        public async Task<IActionResult> AssignRoleToUser(string id, string role)
        {
            await _userService.AssignRoleToUser(id, role);
            return View("Index", _userService.GetApplicationUsers());
        }

        public async Task<IActionResult> RemoveRoleFromUser(string id, string role)
        {
            await _userService.RemoveRoleFromUser(id, role);
            return View("Index", _userService.GetApplicationUsers());
        }
    }
}
