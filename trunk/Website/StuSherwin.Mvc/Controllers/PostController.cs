using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using StuSherwin.Mvc.Filters;
using StuSherwin.Model;
using System.Net;
using System.Text;
using System.IO;
using StuSherwin.Data;
using System.Configuration;

namespace StuSherwin.Mvc.Controllers
{
    [MasterPageDataFilter]
    public class PostController : Controller
    {
        IRecaptchaService _recaptchaService;

        public PostController(IRecaptchaService recaptchaService)
        {
            _recaptchaService = recaptchaService;
        }

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
            ViewBag.AddComment = false;
            return View(post);
        }

        [HttpPost]
        public ActionResult AddComment(int postId, Comment comment, string recaptcha_challenge_field, string recaptcha_response_field)
        {
            var context = new Entities();

            var post = context.Posts
                .Include("Comments")
                .FirstOrDefault(p => p.Id == postId);

            var validationResponse = _recaptchaService.ValidateResponse(recaptcha_challenge_field, recaptcha_response_field, Request.UserHostAddress);

            if (validationResponse == RecaptchaValidationResponse.Failure)
            {
                ModelState.AddModelError("captcha", "You appear to be a robot!");
            }

            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                if (String.IsNullOrWhiteSpace(comment.Author))
                {
                    comment.Author = "Anonymous";
                }
                if (String.IsNullOrWhiteSpace(comment.Title))
                {
                    comment.Title = "Some title";
                }

                post.Comments.Add(comment);
                context.SaveChanges();
                ViewBag.AddComment = false;
                return RedirectToAction("Display", new { id = postId });
            }
            else
            {
                ViewBag.AddComment = true;
                return View("Display", post);
            }
        }

    }
}
