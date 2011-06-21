using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace StuSherwin.Domain
{
    public class XElementFinder
    {
        private XElement _element;
        private XDocument _document;

        public XElementFinder(XElement element)
        {
            _element = element;
        }

        public XElementFinder(XDocument document)
        {
            _document = document;
        }

    }
}
