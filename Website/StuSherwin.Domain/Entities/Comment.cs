using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StuSherwin.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public string Website { get; set; }
        public DateTime Date { get; set; }
        public Post Post { get; set; }
    }
}
