using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Website.Filters;

namespace Website.Controllers
{
    [MasterPageDataFilter]
    public class CodeController : Controller
    {
        //
        // GET: /Code/

        public ActionResult CV()
        {
            return View();
        }

        public ActionResult Download()
        {
            return View();
        }

    }
}
