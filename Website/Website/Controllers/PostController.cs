using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Models;
using System.Data;

namespace Website.Controllers
{
    public class PostController : Controller
    {
        //
        // GET: /Post/

        public ActionResult Index()
        {
            var context = new Entities();
            var posts = context.Posts.ToArray();
            return View(posts);
        }

        public ActionResult Create()
        {
            return View(new Post());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Post post)
        {
            var context = new Entities();
            context.Posts.Add(post);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Display(int id)
        {
            var context = new Entities();
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            return View(post);
        }

        public ActionResult Edit(int id)
        {
            var context = new Entities();
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            return View(post);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Post post)
        {
            var context = new Entities();
            context.Entry(post).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var context = new Entities();
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            return View(post);
        }

        [HttpPost]
        public ActionResult Delete(Post post)
        {
            var context = new Entities();
            context.Entry(post).State = EntityState.Deleted;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
