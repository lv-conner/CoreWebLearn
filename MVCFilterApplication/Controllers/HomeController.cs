using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCFilterApplication.Models;

namespace MVCFilterApplication.Controllers
{
    [Area("System")]
    public class HomeController : Controller
    {
        private readonly ILogger logger;
        public HomeController(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<HomeController>();
        }
        public IActionResult Index()
        {
            logger.LogInformation("Visit Index");
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
    }
}
