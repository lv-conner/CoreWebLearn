using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebSocket.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Set(string key,string message)
        {
            HttpContext.Session.Set(key, Encoding.UTF8.GetBytes(message));
            return Content("Success");
        }


        public IActionResult Get(string key)
        {
            if(HttpContext.Session.TryGetValue(key, out var messageArr))
            {
                return Content(Encoding.UTF8.GetString(messageArr));
            }
            else
            {
                return Content("no exists");
            }
        }
    }
}