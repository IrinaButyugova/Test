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
            var builder = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string>
           {
                {"firstname", "Tom"},
                {"age", "31"}
           })
           .AddJsonFile("conf.json")
           .AddJsonFile("person.json")
           .AddConfiguration(configuration);
            AppConfiguration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRazorPages();
            services.AddTransient<IConfiguration>(provider => AppConfiguration);
            services.Configure<Person>(AppConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // app.UseMiddleware<ConfigMiddleware>();
            
            string projectJsonContent = GetSectionContent(AppConfiguration);
            /* if (env.IsDevelopment())
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
                 await context.Response.WriteAsync("{\n" + projectJsonContent + "}");
             });*/
            app.UseMiddleware<PersonMiddleware>();
            app.UseMiddleware<ConfigMiddleware>();
        }

        private string GetSectionContent(IConfiguration configSection)
        {
            string sectionContent = "";
            foreach (var section in configSection.GetChildren())
            {
                sectionContent += "\"" + section.Key + "\":";
                if (section.Value == null)
                {
                    string subSectionContent = GetSectionContent(section);
                    sectionContent += "{\n" + subSectionContent + "},\n";
                }
                else
                {
                    sectionContent += "\"" + section.Value + "\",\n";
                }
            }
            return sectionContent;
        }
    }
}
