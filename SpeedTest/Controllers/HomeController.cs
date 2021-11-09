using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SpeedTest.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SpeedTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        { 
           return View();
        }

        [HttpGet("/download")]
        public IActionResult Download()
        {
            SendFileAsync().GetAwaiter().GetResult();
            return Ok();
        }

        private async Task SendFileAsync()
        {
            byte[] fileContent = System.IO.File.ReadAllBytes(_configuration.GetValue<string>("FilePath"));
            var fileLength = fileContent.Length;
            HttpContext.Response.Headers.Add("Content-Length", fileLength.ToString());
            await HttpContext.Response.Body.WriteAsync(fileContent, 0, fileLength);
        }

        [HttpPost("/upload")]
        public IActionResult Upload([FromBody]string message)
        {
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
