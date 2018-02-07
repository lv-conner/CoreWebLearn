using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApiTemplate.Controllers.Models;
using WebApiTemplate.ViewModels;

namespace WebApiTemplate.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]/[Action]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public Person GetPerson()
        {
            return new Person()
            {
                Id = "0001",
                Name = "tim"
            };
        }
        [HttpPost]
        public Person SavePerson(Person person)
        {
            return person;
        }
        [HttpGet]
        public List<Person> GetPersonList()
        {
            return new List<Person>()
            {
                new Person()
                {
                    Id="0001",
                    Name="tim"
                },
                new Person()
                {
                    Id="0002",
                    Name="Connner"
                }
            };
        }
        [HttpPost]
        public List<Person> SavePersonList(string list)
        {
            var personList = JsonConvert.DeserializeObject<List<Person>>(list);
            return personList;
        }

        [HttpPost]
        public BookViewModel GetViewModel(BookViewModel model)
        {
            return model;
        }

    }
}