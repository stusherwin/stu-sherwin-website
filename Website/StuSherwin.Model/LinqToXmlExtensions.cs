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
            return from descendent in document.Descendants()
                   where descendent.Name.LocalName == name
                   select descendent;
        }

        public static XElement GetElement(this XElement parentElement, string name)
        {
            return (from element in parentElement.Elements()
                    where element.Name.LocalName == name
                    select element)
                   .FirstOrDefault();
        }

        public static IEnumerable<XElement> GetElements(this XElement parentElement, string name)
        {
            return (from element in parentElement.Elements()
                    where element.Name.LocalName == name
                    select element);
        }

        public static XAttribute GetAttribute(this XElement parentElement, string name)
        {
            return (from id in parentElement.Attributes()
                    where id.Name.LocalName == name
                    select id)
                   .FirstOrDefault();
        }

        public static string GetElementValue(this XElement parentElement, string name)
        {
            var element = parentElement.GetElement(name);
            if (element == null)
                return null;

            return element.Value;
        }

        public static string GetAttributeValue(this XElement parentElement, string name)
        {
            var element = parentElement.GetAttribute(name);
            if (element == null)
                return null;

            return element.Value;
        }
    }
}
