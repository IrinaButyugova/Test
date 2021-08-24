using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ValidationApp.Models;

namespace ValidationApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Person p = new Person
            {
                Name = "Элронд Смит",
                Age = 58,
                HomePage = "www.microsoft.com",
                Email = "elrond.smith@gmail.com",
                Password = "qwerty",
                DateOfBirth = new DateTime(1980, 3, 2)
            };
            return View(p);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Person person)
        {
            if (person.Name == person.Password)
            {
                ModelState.AddModelError("", "Имя и пароль не должны совпадать");
            }

            if (ModelState.IsValid)
                return Content($"{person.Name} - {person.Email}");
            else
                return View(person);
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult CheckEmail(string email)
        {
            if (email == "admin@mail.ru" || email == "aaa@gmail.com")
                return Json(false);
            return Json(true);
        }
    }
}
