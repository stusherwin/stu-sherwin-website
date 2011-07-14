using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StuSherwin.Mvc.Models.Admin
{
    public class EditPost : Model
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }

        [UIHint("HtmlContent")]
        public string Body { get; set; }

        public static EditPost CreateFromPostAndCategories(StuSherwin.Domain.Entities.Post post, IEnumerable<StuSherwin.Domain.Entities.Category> categories)
        {
            return new EditPost
            {
                Title = post.Title,
                CategoryId = post.Category.Id,
                Code = post.Code,
                Body = post.Body,
                Categories = categories
                    .Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    })
            };
        }
    }
}