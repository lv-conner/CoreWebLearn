using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace WebRestfulAPI.Repository
{
    public class MemoryPersonRepository : IPersonRepository
    {
        private readonly ConcurrentBag<Person> _persons = new ConcurrentBag<Person>(); 
        public void Delete(string id)
        {
            var person = _persons.FirstOrDefault(p => p.Id == id);
            if(person!= null)
            {
                if(!_persons.TryTake(out person))
                {
                    throw new InvalidOperationException("删除失败");
                }
                return;
            }
            throw new InvalidOperationException("该对象不存在");
        }

        public Person Get(string id)
        {
            return _persons.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Person> Get()
        {
            return _persons.AsEnumerable();
        }

        public void Save(Person person)
        {
            _persons.Add(person);
        }

        public void Update(Person person)
        {
            var p = Get(person.Id);
            if(p == null)
            {
                throw new InvalidOperationException("该对象不存在");
            }
            else
            {
                var result = _persons.TryTake(out p);
                if(result)
                {
                    _persons.Add(person);
                }
            }
        }
    }
}