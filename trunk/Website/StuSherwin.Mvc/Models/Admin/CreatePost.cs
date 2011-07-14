using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace StuSherwin.Mvc.Models.Admin
{
    public class CreatePost : Model
    {
        public IEnumerable<SelectListItem> Categories { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        [UIHint("HtmlContent")]
        public string Body { get; set; }

        public static CreatePost CreateFromCategories(IEnumerable<StuSherwin.Domain.Entities.Category> categories)
        {
            return new CreatePost
            {
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