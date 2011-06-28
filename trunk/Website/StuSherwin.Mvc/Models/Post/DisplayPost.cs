using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StuSherwin.Mvc.Models.Post
{
    public class DisplayPost
    {
        public string Title { get; set; }
        public DateTime? Published { get; set; }
        public string Body { get; set; }
        public IEnumerable<DisplayComment> Comments { get; set; }
        public AddComment AddComment { get; set; }

        public class DisplayComment
        {
            public string Title { get; set; }
            public string Website { get; set; }
            public string Author { get; set; }
            public DateTime Date { get; set; }
            public string Body { get; set; }
        }

        public static DisplayPost CreateFromPost(StuSherwin.Domain.Entities.Post post, bool redirectToFragment)
        {
            return new DisplayPost
            {
                Title = post.Title,
                Published = post.Published,
                Body = post.Body,
                Comments = post.Comments.Select(c =>
                    new DisplayComment
                    {
                        Title = c.Title,
                        Website = c.Website,
                        Author = c.Author,
                        Date = c.Date,
                        Body = c.Body
                    }),
                AddComment = new AddComment
                {
                    PostId = post.Id,
                    RedirectToFragment = redirectToFragment
                }
            };
        }
    }
}