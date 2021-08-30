using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FiltersApp.Filters
{
    public class DateTimeExecutionFilterAttribute : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers.Add("DateTime", DateTime.Now.ToString());
        }
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }
    }
}
