using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebRestfulAPI.Repository;

namespace WebRestfulAPI.Controllers.V1
{
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonRepository _repository;
        public PersonController(IPersonRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public ActionResult<Person> Get(string id)
        {
            return _repository.Get(id);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Person>> Get()
        {
            return Ok(_repository.Get().ToList());
        }
        [HttpPost]
        public void Post([FromBody]Person person)
        {
            _repository.Save(person);
        }
        [HttpPut]
        public ActionResult<Person> Put(Person person)
        {
            _repository.Update(person);
            return person;
        }
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _repository.Delete(id);
        }


    }
}