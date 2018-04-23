using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace WebSocket.Controllers
{
    public class CacheController : Controller
    {
        private readonly IDistributedCache cache;
        public CacheController(IDistributedCache cache)
        {
            this.cache = cache;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Set(string key,string value)
        {
            try
            {
                await cache.SetStringAsync(key, value);
                return Content("Success");
            }
            catch(Exception e)
            {
                return Content(e.Message);
            }

        }

        public async Task<IActionResult> Get(string key)
        {
            return Content(await cache.GetStringAsync(key));
        }
    }
}