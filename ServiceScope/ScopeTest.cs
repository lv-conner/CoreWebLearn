using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ServiceScope
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ScopeTest
    {
        private readonly RequestDelegate _next;

        public ScopeTest(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext httpContext,ITransient transient, IScope scope, ISingleton singleton)
        {
            transient.Count++;
            scope.Count++;
            singleton.Count++;
            //await httpContext.Response.WriteAsync(string.Format("transient:\t{0},scope:\t{1},singleton:\t{2}", transient.Count, scope.Count, singleton.Count));
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ScopeTestExtensions
    {
        public static IApplicationBuilder UseScopeTest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ScopeTest>();
        }
    }
}
