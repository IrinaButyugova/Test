using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Test.Logging;
using Test.Routing;

namespace Test
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; set; }

        public Startup(IConfiguration configuration)
        {
            AppConfiguration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache()
            .AddSession(options =>
            {
                options.Cookie.Name = ".MyApp.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.IsEssential = true;
            })
            .AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            var logger = loggerFactory.CreateLogger("FileLogger");

            var myRouteHandler = new RouteHandler(HandleAsync);
            var routeBuilder = new RouteBuilder(app, myRouteHandler);
            routeBuilder.Routes.Add(new AdminRoute());
            routeBuilder.MapRoute("{controller}/{action}",
               async context =>
               {
                   context.Response.ContentType = "text/html;charset=utf-8";
                   await context.Response.WriteAsync("двухсегментный запрос");
               });
            routeBuilder.MapRoute("default",
            "{controller}/{action}/{id?}",
            null,
            new { myConstraint = new CustomConstraint("/Home/Index/12") });

            app.UseRouter(routeBuilder.Build());

            app.Use(async (context, next) =>
            {
                context.Items["text"] = "Text from HttpContext.Items";
                await next.Invoke();
            })
                .UseSession();

            app.Run(async (context) =>
            {
                string result = "";

                context.Response.ContentType = "text/html; charset=utf-8";
                if (context.Items.ContainsKey("text"))
                {
                    result += $"{context.Items["text"]}";
                }

                string textFromCookies = "";
                if (context.Request.Cookies.TryGetValue("text", out textFromCookies))
                {
                    result += $"; {textFromCookies}";
                }
                else
                {
                    context.Response.Cookies.Append("text", "Text from cookies");
                    result += "; Cookies text is not found";
                }

                if (context.Session.Keys.Contains("person"))
                {
                    Person person = context.Session.Get<Person>("person");
                    result += $"; Hello {person.Name}, your age: {person.Age}!";
                }
                else
                {
                    Person person = new Person { Name = "Tom", Age = 22 };
                    context.Session.Set<Person>("person", person);
                    result += " ; There no person in the session.";
                }

                logger.LogInformation("Processing request {0}", context.Request.Path);
                await context.Response.WriteAsync(result);

            });
        }

        private async Task HandleAsync(HttpContext context)
        {
            var routeValues = context.GetRouteData().Values;
            var action = routeValues["action"].ToString();
            var controller = routeValues["controller"].ToString();
            string id = routeValues["id"]?.ToString();
            await context.Response.WriteAsync($"controller: {controller} | action: {action} | id: {id}");
        }
    }
}
