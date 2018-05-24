using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookieAuthentication.Controllers
{
    public class CookieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Set(string key,string value)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions()
            {
                HttpOnly = true,
                Secure = false,
                Path = "/"
            });
            return Redirect("/Cookie/Get?key=" + key);
        }

        public IActionResult Get(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out var value);
            return Redirect("/Home/Index");
        }
    }
}