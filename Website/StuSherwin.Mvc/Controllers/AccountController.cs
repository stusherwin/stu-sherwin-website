using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StuSherwin.Domain.Repositories;
using StuSherwin.Mvc.Models.Account;
using System.Web.Security;

namespace StuSherwin.Mvc.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Login(string username, string password)
        {
            var model = new Login
            {
                Username = username,
                Password = password,
            };
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login, string returnUrl)
        {
            var user = _userRepository.FindByUsername(login.Username);

            if (user == null || !user.IsValidPassword(login.Password))
                ModelState.AddModelError("password", "Username or password is incorrect");

            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(user.Username, true);
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index");
            }
            return Login(login.Username, login.Password);
        }

        public ActionResult Logout(string returnUrl)
        {
            FormsAuthentication.SignOut();
            return Redirect(returnUrl);
        }
    }
}
