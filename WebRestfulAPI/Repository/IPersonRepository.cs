using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebRestfulAPI.Repository
{
    public interface IPersonRepository
    {
        Person Get(string id);
        IEnumerable<Person> Get();
        void Save(Person person);
        void Update(Person person);
        void Delete(string id);
    }
}
