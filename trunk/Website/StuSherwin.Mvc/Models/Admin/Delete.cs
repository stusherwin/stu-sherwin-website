using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StuSherwin.Mvc.Models.Admin
{
    public class Delete : Model
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public static Delete CreateFromPost(StuSherwin.Domain.Entities.Post post)
        {
            return new Delete
            {
                Id = post.Id,
                Title = post.Title
            };
        }
    }
}