using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookieAuthentication.Controllers
{
    public class FileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Upload(SampleInputModel model)
        {
            return Content(HttpContext.Request.Form.Files.ToList().Count.ToString());
        }
    }


    public class SampleInputModel
    {
        public IFormFile fileUpload { get; set; }
        public string name { get; set; }
    }
}