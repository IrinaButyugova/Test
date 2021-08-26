using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages
{
    public class PersonModel : PageModel
    {
        public string Message { get; set; }

        [BindProperty]
        public Person Person { get; set; }
        public void OnGet()
        {
            Message = "������� ������";
        }
        public void OnPost()
        {
            Message = $"���: {Person.Name}  �������: {Person.Age}";
        }
    }
}
