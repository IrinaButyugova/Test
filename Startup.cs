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

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string>
           {
                {"firstname", "Tom"},
                {"age", "31"}
           })
           .AddJsonFile("conf.json"); ;
            AppConfiguration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            AppConfiguration["firstname"] = "alice";
            AppConfiguration["lastname"] = "simpson";
            string java_dir = AppConfiguration["JAVA_HOME"] ?? "not set";
            var color = AppConfiguration["color"];
            var text = AppConfiguration["text"];
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"{AppConfiguration["firstname"]} {AppConfiguration["lastname"]} - {AppConfiguration["age"]}");
                await context.Response.WriteAsync(java_dir);
                await context.Response.WriteAsync($"<p style='color:{color};'>{text}</p>");
            });
        }
    }
}
