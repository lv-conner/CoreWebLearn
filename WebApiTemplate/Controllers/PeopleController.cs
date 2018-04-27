using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTemplate.Controllers
{
    [Produces("application/json")]
    [Route("api/People")]
    public class PeopleController : Controller
    {
    }
}