using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StuSherwin.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
