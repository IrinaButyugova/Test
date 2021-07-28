using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;

namespace Test
{
    public class PersonMiddleware
    {
        private readonly RequestDelegate _next;

        public Person Person { get; }

        public PersonMiddleware(RequestDelegate next, IOptions<Person> options)
        {
            _next = next;
            Person = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"<p>Name: {Person?.Name}</p>");
            stringBuilder.Append($"<p>Age: {Person?.Age}</p>");
            stringBuilder.Append($"<p>Company: {Person?.Company?.Title}</p>");
            stringBuilder.Append("<h3>Languages</h3><ul>");
            foreach (string lang in Person.Languages)
                stringBuilder.Append($"<li>{lang}</li>");
            stringBuilder.Append("</ul>");

            await context.Response.WriteAsync(stringBuilder.ToString());
        }
    }
}
