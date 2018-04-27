using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSocket.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebSocket.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment environment;
        public HomeController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }
        public IActionResult Index()
        {
            return Content(Directory.GetCurrentDirectory() + "\t" + environment.ContentRootPath);
            //return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
