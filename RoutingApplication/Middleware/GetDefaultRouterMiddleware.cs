using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace RoutingApplication.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GetDefaultRouterMiddleware
    {
        private readonly RequestDelegate _next;

        public GetDefaultRouterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext,IRouter router)
        {
            var routerStr = router.GetType().ToString();
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GetDefaultRouterMiddlewareExtensions
    {
        public static IApplicationBuilder UseGetDefaultRouterMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GetDefaultRouterMiddleware>();
        }
    }
}
