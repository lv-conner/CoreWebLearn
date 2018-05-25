using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CookieAuthentication.Controllers
{
    public class ServiceInjectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add([FromServices]IMemoryCache cache,string key,string value)
        {
            cache.Set<string>(key, value);
            return Content("success");
        }
    }
}