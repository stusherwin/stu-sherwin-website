using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StuSherwin.Mvc.Models.Post
{
    public class List : Model
    {
        public IEnumerable<PostItem> Posts { get; set; }

        public static List CreateFromPosts(IEnumerable<StuSherwin.Domain.Entities.Post> posts)
        {
            return new List
            {
                Posts = posts.Select(p => PostItem.CreateFromPost(p)).ToArray()
            };
        }

        public class PostItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Code { get; set; }
            public DateTime? Published { get; set; }
            public string Body { get; set; }
            public int CommentCount { get; set; }

            public static PostItem CreateFromPost(StuSherwin.Domain.Entities.Post post)
            {
                return new PostItem
                {
                    Id = post.Id,
                    Title = post.Title,
                    Code = post.Code,
                    Published = post.Published,
                    Body = post.Body,
                    CommentCount = post.Comments.Count
                };
            }
        }
    }
}