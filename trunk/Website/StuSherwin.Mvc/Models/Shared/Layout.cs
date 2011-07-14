using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StuSherwin.Mvc.Models.Shared
{
    public class Layout
    {
        public enum LayoutType
        {
            Home,
            Post,
            Admin
        }

        public string Category { get; set; }
        public string Page { get; set; }
        public string PostCode { get; set; }
        public LayoutType Type { get; set; }
    }
}