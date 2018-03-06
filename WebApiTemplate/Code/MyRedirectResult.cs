using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTemplate.Code
{
    public class MyRedirectResult : IActionResult
    {
        private readonly string _url;
        public MyRedirectResult(string url)
        {
            _url = url;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = 302;
            context.HttpContext.Response.Headers.Add("location", _url);
            //await context.HttpContext.Response.WriteAsync("");
            await Task.CompletedTask;
        }
    }
}
