using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace StuSherwin.Model
{
    public static class LinqToXmlExtensions
    {
        public static IEnumerable<XElement> GetDescendants(this XDocument document, string name)
        {
            return from d in document.Descendants()
                   where d.Name.LocalName == name
                   select d;
        }

        public static XElement GetElement(this XElement element, string name)
        {
            return (from id in element.Elements()
                    where id.Name.LocalName == name
                    select id)
                   .FirstOrDefault();
        }

        public static XAttribute GetAttribute(this XElement element, string name)
        {
            return (from id in element.Attributes()
                    where id.Name.LocalName == name
                    select id)
                   .FirstOrDefault();
        }
    }
}
