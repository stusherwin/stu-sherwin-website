using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StuSherwin.Mvc.Models.Admin
{
    public class Publish : Model
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Display(Name = "Publish date")]
        public DateTime PublishDate { get; set; }

        public static Publish CreateFromPost(StuSherwin.Domain.Entities.Post post)
        {
            return new Publish
            {
                Id = post.Id,
                Title = post.Title,
                PublishDate = DateTime.Now
            };
        }
    }
}