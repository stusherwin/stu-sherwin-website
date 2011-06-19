using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StuSherwin.Mvc.Filters;

namespace StuSherwin.Mvc.Controllers
{
    [MasterPageDataFilter]
    public class MusicController : Controller
    {
        //
        // GET: /Music/

        public ActionResult Page1()
        {
            return View();
        }

    }
}
