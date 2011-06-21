using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StuSherwin.Model
{
    public class HtmlParseException : Exception
    {
        public HtmlParseException(string message) : base(message)
        { 
        }
    }
}
