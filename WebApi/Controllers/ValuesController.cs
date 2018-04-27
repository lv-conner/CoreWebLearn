using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public static Dictionary<string,object> resources = new Dictionary<string,object>();
        // GET api/values
        [HttpGet]
        public Dictionary<string, object> Get()
        {
            return resources;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            resources.TryGetValue(id, out var value);
            return value.ToString();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
