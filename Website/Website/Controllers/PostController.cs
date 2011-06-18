using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Data;
using Website.Filters;
using StuSherwin.Poco;

namespace Website.Controllers
{
    [MasterPageDataFilter]
    public class PostController : Controller
    {
        //
        // GET: /Post/

        public ActionResult Index(string category)
        {
            var context = new Entities();
            var posts = context.Posts
                .Include("Comments")
                .Include("Category")
                .Where(p => p.Category.Code == category)
                .ToArray();
            return View(posts);
        }

        public ActionResult Display(int id)
        {
            var context = new Entities();
            var post = context.Posts
                .Include("Comments")
                .FirstOrDefault(p => p.Id == id);
            return View(post);
        }

        [HttpPost]
        public ActionResult AddComment(int postId, string name, string website, string title, string body)
        {
            var context = new Entities();
            var post = context.Posts
                .Include("Comments")
                .FirstOrDefault(p => p.Id == postId);

            var comment = new Comment
            {
                Author = name,
                Title = title,
                Website = website,
                Body = body,
                Date = DateTime.Now
            };

            post.Comments.Add(comment);
            context.SaveChanges();
            return RedirectToAction("Display", new { id = postId });
        }
    }
}
