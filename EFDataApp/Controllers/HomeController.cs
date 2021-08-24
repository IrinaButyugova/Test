using EFDataApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ApplicationContext _db;
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _db = context;

            if (_db.Companies.Count() == 0)
            {
                Company oracle = new Company { Name = "Oracle" };
                Company google = new Company { Name = "Google" };
                Company microsoft = new Company { Name = "Microsoft" };
                Company apple = new Company { Name = "Apple" };

                User user1 = new User { Name = "Олег Васильев", Company = oracle, Age = 26 };
                User user2 = new User { Name = "Александр Овсов", Company = oracle, Age = 24 };
                User user3 = new User { Name = "Алексей Петров", Company = microsoft, Age = 25 };
                User user4 = new User { Name = "Иван Иванов", Company = microsoft, Age = 26 };
                User user5 = new User { Name = "Петр Андреев", Company = microsoft, Age = 23 };
                User user6 = new User { Name = "Василий Иванов", Company = google, Age = 23 };
                User user7 = new User { Name = "Олег Кузнецов", Company = google, Age = 25 };
                User user8 = new User { Name = "Андрей Петров", Company = apple, Age = 24 };

                _db.Companies.AddRange(oracle, microsoft, google, apple);
                _db.Users.AddRange(user1, user2, user3, user4, user5, user6, user7, user8);
                _db.SaveChanges();
            }
        }

        public async Task<IActionResult> Index(int? company, string name, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 3;

            IQueryable<User> users = _db.Users.Include(x => x.Company);
            if (company != null && company != 0)
            {
                users = users.Where(p => p.CompanyId == company);
            }
            if (!String.IsNullOrEmpty(name))
            {
                users = users.Where(p => p.Name.Contains(name));
            }

            var count = await users.CountAsync();
            var items = await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["AgeSort"] = sortOrder == SortState.AgeAsc ? SortState.AgeDesc : SortState.AgeAsc;
            ViewData["CompSort"] = sortOrder == SortState.CompanyAsc ? SortState.CompanyDesc : SortState.CompanyAsc;

            users = sortOrder switch
            {
                SortState.NameDesc => users.OrderByDescending(s => s.Name),
                SortState.AgeAsc => users.OrderBy(s => s.Age),
                SortState.AgeDesc => users.OrderByDescending(s => s.Age),
                SortState.CompanyAsc => users.OrderBy(s => s.Company.Name),
                SortState.CompanyDesc => users.OrderByDescending(s => s.Company.Name),
                _ => users.OrderBy(s => s.Name),
            };

            List<Company> companies = _db.Companies.ToList();
            companies.Insert(0, new Company { Name = "Все", Id = 0 });

            IndexViewModel viewModel = new IndexViewModel
            {
                Users = items,
                SortViewModel = new SortViewModel(sortOrder),
                Companies = new SelectList(companies, "Id", "Name"),
                Name = name,
                PageViewModel = pageViewModel
            };
            return View(viewModel);
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
        public async Task<IActionResult> Create(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                User user = await _db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User user = await _db.Users.Include(x => x.Company).FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            User existingUser = await _db.Users.FirstOrDefaultAsync(p => p.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Name = user.Name;
                existingUser.Age = user.Age;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await _db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                if (id != null)
                {
                    User user = new User { Id = id.Value };
                    _db.Entry(user).State = EntityState.Deleted;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}
