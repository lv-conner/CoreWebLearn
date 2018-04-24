using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebSocket.Controllers
{
    public class BindController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}