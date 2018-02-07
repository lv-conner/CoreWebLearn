using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTemplate.Controllers.Models;
using WebApiTemplate.Models;

namespace WebApiTemplate.ViewModels
{
    public class BookViewModel
    {
        public Person Header { get; set; }
        public List<Book> BookList { get; set; }
    }
}
