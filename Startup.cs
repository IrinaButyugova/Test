using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".MyApp.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
                    await context.Response.WriteAsync(result);

            });
        }
    }
}
