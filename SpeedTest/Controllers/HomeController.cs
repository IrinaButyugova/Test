using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpeedTest.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpeedTest.Controllers
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
           return View();
        }

        private async Task SendFileAsync()
        {
            byte[] fileContent = System.IO.File.ReadAllBytes("c:/git/jquery1.txt");
            var fileLength = fileContent.Length;
            HttpContext.Response.Headers.Add("Content-Length", fileLength.ToString());
            await HttpContext.Response.Body.WriteAsync(fileContent, 0, fileLength);
        }

        [HttpGet("/download")]
        public IActionResult Download()
        {
            SendFileAsync().GetAwaiter().GetResult();
            return Ok();
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
    }
}
