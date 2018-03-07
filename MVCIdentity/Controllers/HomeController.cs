﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MVCIdentity.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis.Options;
using MVCIdentity.Code;
using Microsoft.Extensions.Options;

namespace MVCIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptions<SecretManager> _option;
        public HomeController(IOptions<SecretManager> option)
        {
            _option = option;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult GetPassword()
        {
            return Content(_option.Value.Password);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Info()
        {
            return Json(HttpContext.RequestServices.GetService<IHostingEnvironment>());
        }

        public IActionResult List(List<string> list)
        {
            return Json(list);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
