using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using System.Text;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewWebMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SetSession(string id)
        {
            byte[] value = Encoding.UTF8.GetBytes(id);
            try
            {
                HttpContext.Session.Set("id", value);
                return Content("success");
            }
            catch(Exception e)
            {
                return Content(e.Message + e.StackTrace);
            }
        }

        public IActionResult GetSession(string id)
        {
            byte[] value;
            if(HttpContext.Session.TryGetValue(id,out value))
            {
                var key = Encoding.UTF8.GetString(value);
                return Content(key);
            }
            else
            {
                return Content("no exists");
            }

        }
    }
}
