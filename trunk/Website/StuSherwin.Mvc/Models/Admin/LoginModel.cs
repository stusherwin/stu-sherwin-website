using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StuSherwin.Mvc.Models.Admin
{
    public class LoginModel
    {
        public string Username { get; set; }
        [UIHint("Password")]
        public string Password { get; set; }
    }
}