using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Data;

namespace Website.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Post/

        public ActionResult Index()
        {
            var context = new Entities();
            var posts = context.Posts
                .Include("Comments")
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
