﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StuSherwin.Mvc.Models.Post
{
    public class AddComment : Model
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Did you forget to write your comment?")]
        public string Body { get; set; }
        public string Author { get; set; }
        [RegularExpression(@"^[^\.\:]+(\.[^\.\:]+)+(/[^/:]+)*/?$", ErrorMessage = "You may have typed your website address incorrectly")]
        public string Website { get; set; }
        public string recaptcha_challenge_field { get; set; }
        [Required(ErrorMessage = "You didn't do the robot filter thingy")]
        public string recaptcha_response_field { get; set; }
        public bool RedirectToFragment { get; set; }
    }
}