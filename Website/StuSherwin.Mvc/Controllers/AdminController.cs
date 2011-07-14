using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StuSherwin.Mvc.Models.Admin;
using System.Web.Security;
using System.Data;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using StuSherwin.Domain;
using StuSherwin.Data;
using StuSherwin.Domain.Entities;
using StuSherwin.Domain.Importing;

namespace StuSherwin.Mvc.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            var context = new Entities();
            var categories = context.Categories.Include("Posts").ToArray();
            var model = StuSherwin.Mvc.Models.Admin.Index.CreateFromCategories(categories);
            return View(model);
        }

        public ActionResult Create()
        {
            var context = new Entities();
            var categories = context.Categories.ToArray();
            var model = CreatePost.CreateFromCategories(categories);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CreatePost model)
        {
            var context = new Entities();
            var category = context.Categories.FirstOrDefault(c => c.Id == model.CategoryId);
            var post = new Post
            {
                Title = model.Title,
                Category = category,
                Code = model.Code,
                Body = model.Body
            };
            context.Posts.Add(post);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var context = new Entities();
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            var categories = context.Categories.ToArray();
            var model = EditPost.CreateFromPostAndCategories(post, categories);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(EditPost model)
        {
            var context = new Entities();
            var category = context.Categories.FirstOrDefault(c => c.Id == model.CategoryId);
            var post = context.Posts.FirstOrDefault(p => p.Id == model.Id);
            
            post.Title = model.Title;
            post.Category = category;
            post.Code = model.Code;
            post.Body = model.Body;

            context.Entry(post).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var context = new Entities();
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            var model = StuSherwin.Mvc.Models.Admin.Delete.CreateFromPost(post);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(StuSherwin.Mvc.Models.Admin.Delete model)
        {
            var context = new Entities();
            var post = context.Posts.FirstOrDefault(p => p.Id == model.Id);
            context.Entry(post).State = EntityState.Deleted;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Publish(int id)
        {
            var context = new Entities();
            var post = context.Posts.FirstOrDefault(p => p.Id == id);
            var model = StuSherwin.Mvc.Models.Admin.Publish.CreateFromPost(post);
            return View(model);
        }

        [HttpPost]
        public ActionResult Publish(StuSherwin.Mvc.Models.Admin.Publish model)
        {
            var context = new Entities();
            var post = context.Posts.FirstOrDefault(p => p.Id == model.Id);
            post.Published = model.PublishDate;
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                var context = new Entities();
                var importer = new PostImporter();
                var category = context.Categories.FirstOrDefault(c => c.Code == "Philosophy");
                foreach (var post in importer.ImportPosts(file.InputStream, category))
                {
                    context.Posts.Add(post);
                }
                context.SaveChanges();
            }
            return View();
        }       
    }
}
