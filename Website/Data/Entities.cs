using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using StuSherwin.Poco;

namespace Data
{
    public class Entities : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
