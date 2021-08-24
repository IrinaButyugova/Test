using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ViewComponentTest.Components
{
    public class UsersListViewComponent : ViewComponent
    {
        List<string> users;
        public UsersListViewComponent()
        {
            users = new List<string>
            {
                "Tom", "Tim", "Bob", "Sam", "Alice", "Kate"
            };
        }
        public IViewComponentResult Invoke()
        {
            int number = users.Count;
            // если передан параметр number
            if (Request.Query.ContainsKey("number"))
            {
                Int32.TryParse(Request.Query["number"].ToString(), out number);
            }

            ViewBag.Users = users.Take(number);
            ViewData["Header"] = $"Количество пользователей: {number}.";
            return View("Users");
        }
    }
}
