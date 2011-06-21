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
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        [Authorize]
        public ActionResult Index()
        {
            var context = new Entities();
            var posts = context.Posts.ToArray();
            return View(posts);
        }

        public ActionResult Login(string username, string password)
        {
            var model = new LoginModel
            {
                Username = username,
                Password = password,
            };
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie("bob", true);
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index");
            }
            return Login(login.Username, login.Password);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
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
                var category = context.Categories.FirstOrDefault(c => c.Code == "Music");
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
