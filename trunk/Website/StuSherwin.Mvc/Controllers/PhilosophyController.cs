using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Filters;

namespace Website.Controllers
{
    [MasterPageDataFilter]
    public class PhilosophyController : Controller
    {
        //
        // GET: /Philosophy/

        public ActionResult Page1()
        {
            return View();
        }

    }
}
