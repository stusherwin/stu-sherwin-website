using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using StuSherwin.Domain;
using System.Net;
using System.Text;
using System.IO;
using StuSherwin.Data;
using System.Configuration;
using StuSherwin.Mvc.Models.Post;
using StuSherwin.Mvc.Core;
using StuSherwin.Domain.Recaptcha;
using StuSherwin.Domain.Entities;
using StuSherwin.Domain.Importing;
using StuSherwin.Domain.Repositories;

namespace StuSherwin.Mvc.Controllers
{
    public class PostController : Controller
    {
        IRecaptchaService _recaptchaService;
        IPostRepository _postRepository;

        public PostController(IPostRepository postRepository, IRecaptchaService recaptchaService)
        {
            _recaptchaService = recaptchaService;
            _postRepository = postRepository;
        }

        //
        // GET: /Post/

        public ActionResult Index(string category)
        {
            var posts = _postRepository.FindAllPublishedByCategoryCode(category);
            var model = Models.Post.List.CreateFromPosts(posts);
            return View(model);
        }

        public ActionResult Display(string code)
        {
            var post = _postRepository.FindByCode(code);
            var model = Models.Post.Display.CreateFromPost(post, false);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddComment(AddComment addComment)
        {
            var post = _postRepository.FindById(addComment.PostId);

            if (!String.IsNullOrEmpty(addComment.recaptcha_response_field))
            {
                var validationResponse = _recaptchaService.ValidateResponse(addComment.recaptcha_challenge_field, addComment.recaptcha_response_field, Request.UserHostAddress);

                if (validationResponse == RecaptchaValidationResponse.Failure)
                {
                    ModelState.AddModelError("captcha", "Google thinks you're a robot!  Do you want to try again?");
                }
            }

            if (ModelState.IsValid)
            {
                Comment comment = new Comment();
                comment.Post = post;
                comment.Author = addComment.Author;
                comment.Title = addComment.Title;
                comment.Body = addComment.Body;
                comment.Website = addComment.Website;
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
                _postRepository.SaveChanges();

                return RedirectToAction("Display", new { code = post.Code, category = post.Category.Name });
            }
            else
            {
                var model = Models.Post.Display.CreateFromPost(post, true);
                return View("Display", model);
            }
        }

        public ActionResult Redirect(string year, string month, string title)
        {
            var post = _postRepository.FindByOldUrl("http://blog.stusherwin.com/"
                    + year
                    + "/"
                    + month
                    + "/"
                    + title
                    + ".html");

            if (post == null)
                throw new HttpException(404, "Page not found");
            
            return new PermanentRedirectResult(Url.Action("Display", new { code = post.Code, category="Philosophy" }));
        }

        /*public ActionResult Test()
        {
            var converter = new HtmlConverter();
            converter.LoadHtml("This is some text.<br /><br />This is some more text.");
            converter.ConvertDoubleBrTagsToParagraphTags();
            return new RedirectResult("/");
        }*/
    }
}
