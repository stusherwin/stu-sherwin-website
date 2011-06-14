using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Data;
using Website.Filters;

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
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            return View(post);
        }
    }
}
