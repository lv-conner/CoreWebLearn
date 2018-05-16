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
        private readonly IApplicationLifetime applicationLifetime;
        public HomeController(IHostingEnvironment environment,IApplicationLifetime applicationLifetime)
        {
            this.environment = environment;
            this.applicationLifetime = applicationLifetime;
        }
        public IActionResult Index()
        {
            //return Content(Directory.GetCurrentDirectory() + "\t" + environment.ContentRootPath);
            var features = HttpContext.Features;
            return View();
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

        public IActionResult StopApplication()
        {
            applicationLifetime.StopApplication();
            return Content("Success");
        }
    }
}
