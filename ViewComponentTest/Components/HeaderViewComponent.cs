using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Html;

namespace ViewComponentTest.Components
{
    public class HeaderViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string htmlContent = String.Empty;
            using (StreamReader reader = new StreamReader("Files/header.html"))
            {
                htmlContent = await reader.ReadToEndAsync();
            }
            return new HtmlContentViewComponentResult(new HtmlString(htmlContent));
        }
    }
}
