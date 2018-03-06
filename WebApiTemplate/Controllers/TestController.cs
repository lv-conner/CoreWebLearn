using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTemplate.Controllers
{
    [Produces("application/json")]
    [Route("api/Test/[Action]/{id?}")]
    public class TestController : Controller
    {
        public TestController()
        {

        }
        [HttpGet]
        public IActionResult Test(string id)
        {
            return Content(id);
        }

    }
}