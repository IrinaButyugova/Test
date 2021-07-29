using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace Test.Routing
{
    public class AdminRoute : IRouter
    {
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            throw new NotImplementedException();
        }

        public async Task RouteAsync(RouteContext context)
        {
            string url = context.HttpContext.Request.Path.Value.TrimEnd('/');
            if (url.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase))
            {
                context.Handler = async ctx =>
                {
                    ctx.Response.ContentType = "text/html;charset=utf-8";
                    await ctx.Response.WriteAsync("Привет admin!");
                };
            }
            await Task.CompletedTask;
        }
    }
}
