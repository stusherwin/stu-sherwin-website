using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StuSherwin.Mvc.Models.Shared
{
    public class Layout
    {
        public string Category { get; set; }
        public string Page { get; set; }
        public int? PostId { get; set; }
        public bool IsHomePage { get; set; }
    }
}