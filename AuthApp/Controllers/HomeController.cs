using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace AuthApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = "admin, user")]
        public IActionResult Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            return Content($"ваша роль: {role}");
        }

        [Authorize(Roles = "admin")]
        public IActionResult About()
        {
            return Content("Вход только для администратора");
        }

        [Authorize(Policy = "OnlyForLondon")]
        public IActionResult TestCityPolicy()
        {
            return View();
        }

        [Authorize(Policy = "OnlyForMicrosoft")]
        public IActionResult TestCompanyPolicy()
        {
            return Content("Only for Microsoft employees");
        }

        [Authorize(Policy = "AgeLimit")]
        public IActionResult TestAgeLimitPolicy()
        {
            return Content("Only for ages in limit.");
        }
    }
}
