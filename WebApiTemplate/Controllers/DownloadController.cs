using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using WebApiTemplate.Code;

namespace WebApiTemplate.Controllers
{
    [Produces("application/json")]
    [Route("api/Download/[action]")]
    public class DownloadController : Controller
    {
        //private readonly IFileProvider fileProvider;
        public DownloadController(/*IFileProvider fileProvider*/)
        {
            //this.fileProvider = fileProvider;
        }
        [HttpGet]
        public IActionResult DownLoadFile()
        {
            return File("Hello.txt", "html/txt","Hello.txt");

        }
        [HttpGet]
        public IActionResult GetAction()
        {
            return new EmptyJsonContentResult();
        }

        public IActionResult Redirect()
        {
            return new RedirectResult("/api/DownLoad/GetAction", false, false);
        }

        public IActionResult GetResponseType()
        {
            return Content(this.HttpContext.Response.GetType().ToString());
        }

        public IActionResult Red()
        {
            return new MyRedirectResult("/api/Download/GetResponseType");
        }


    }
}