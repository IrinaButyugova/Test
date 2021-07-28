using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Test
{
    public class ConfigMiddleware
    {
        private readonly RequestDelegate _next;
        public IConfiguration AppConfiguration { get; set; }

        public ConfigMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            AppConfiguration = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await context.Response.WriteAsync($"name: {AppConfiguration["firstname"]}  age: {AppConfiguration["age"]}");
        }
    }
}
