using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StuSherwin.Mvc.Models.Admin
{
    public class Index : Model
    {
        public IEnumerable<Category> Categories { get; set; }

        public class Category
        {
            public string Name { get; set; }
            public IEnumerable<Post> Posts { get; set; }
        }

        public class Post
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Code { get; set; }
            public DateTime? Published { get; set; }
        }

        public static Index CreateFromCategories(IEnumerable<StuSherwin.Domain.Entities.Category> categories)
        {
            return new Index
            {
                Categories = categories.Select(c => new Category {
                    Name = c.Name,
                    Posts = c.Posts.Select(p => new Post {
                        Id = p.Id,
                        Title = p.Title,
                        Code = p.Code,
                        Published = p.Published
                    })
                })
            };
        }
    }
}