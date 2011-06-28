using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace StuSherwin.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        [UIHint("HtmlContent")]
        public string Body { get; set; }
        public DateTime? Published { get; set; }
        public DateTime? Updated { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Category Category { get; set; }
        public string OldUrl { get; set; }
    }
}
