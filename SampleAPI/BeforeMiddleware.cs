using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SampleAPI
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class BeforeMiddleware
    {
        private readonly RequestDelegate _next;

        public BeforeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, IApiDescriptionGroupCollectionProvider provier)
        {
            var apiCount =  provier.ApiDescriptionGroups.Items.Count;
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class BeforeMiddlewareExtensions
    {
        public static IApplicationBuilder UseBeforeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BeforeMiddleware>();
        }
    }
}
