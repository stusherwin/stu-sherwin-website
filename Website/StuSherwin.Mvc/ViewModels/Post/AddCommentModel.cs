using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StuSherwin.Mvc.ViewModels.Post
{
    public class AddCommentModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Author { get; set; }
        public string Website { get; set; }
        public string recaptcha_challenge_field { get; set; }
        public string recaptcha_response_field { get; set; }
    }
}