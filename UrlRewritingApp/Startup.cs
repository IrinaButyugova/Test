using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlRewritingApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var options = new RewriteOptions()
                .AddRedirect("(.*)/$", "$1")
                .AddRewrite(@"home/index/(\d+)", "home/about?id=$1", skipRemainingRules: false)
                .AddRewrite(@"product/(\w+)/(\d+)", "home/products?cat=$1&id=$2", skipRemainingRules: false)
                .AddIISUrlRewrite(env.ContentRootFileProvider, "urlrewrite.xml")
                .Add(new RedirectPHPRequests(extension: "html", newPath: "/html"));
            app.UseRewriter(options);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                endpoints.MapGet("/home/index", async context =>
                {
                    await context.Response.WriteAsync("Home Index Page!");
                });
                endpoints.MapGet("/home/about", async context =>
                {
                    await context.Response.WriteAsync($"About: {context.Request.Path}");
                });
                endpoints.MapGet("/home/products", async context =>
                {
                    await context.Response.WriteAsync($"cat: {context.Request.Query["cat"].ToString()}  " +
                        $"id: {context.Request.Query["id"].ToString()}");
                });
                endpoints.MapGet("/home/items", async context =>
                {
                    await context.Response.WriteAsync($"Values:  id = {context.Request.Query["id"]} " +
                            $"name = {context.Request.Query["name"]}");
                });
            });
        }
    }
}
