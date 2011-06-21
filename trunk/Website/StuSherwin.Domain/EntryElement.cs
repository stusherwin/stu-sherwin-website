using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace StuSherwin.Domain
{
    public class EntryElement
    {
        public string ElementId { get; set; }
        public DateTime Published { get; set; }
        public DateTime Updated { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string PostElementId { get; set; }
        public string Author { get; set; }
        public string AuthorUri { get; set; }
        public string PostUri { get; set; }

        public static IEnumerable<EntryElement> GetAllEntries(XDocument document)
        {
            return from d in document.GetDescendants("entry")
                   let elementId = GetPostIdValue(d.GetElementValue("id"))
                   where elementId != null
                   select new EntryElement
                   {
                       ElementId = elementId,
                       Published = DateTime.Parse(d.GetElementValue("published")),
                       Updated = DateTime.Parse(d.GetElementValue("updated")),
                       Category = GetCategoryValue(d.GetElement("category").GetAttributeValue("term")),
                       Title = d.GetElementValue("title"),
                       Content = d.GetElementValue("content"),
                       Author = d.GetElement("author").GetElementValue("name"),
                       AuthorUri = d.GetElement("author").GetElementValue("uri"),
                       PostElementId = GetPostId(d),
                       PostUri = GetPostUri(d)
                   };
        }

        private static string GetPostUri(XElement element)
        {
            var link = element.GetElements("link")
                .FirstOrDefault(l => 
                    l.GetAttribute("rel") != null
                    && l.GetAttribute("rel").Value == "alternate");
            
            if (link == null)
                return null;

            return link.GetAttributeValue("href");
        }

        private static string GetPostId(XElement element)
        {
            var inReplyToElement = element.GetElement("in-reply-to");
            if (inReplyToElement == null)
                return null;

            return GetPostIdValue(inReplyToElement.GetAttributeValue("ref"));
        }

        private static string GetPostIdValue(string value)
        {
            var postIdMatch = Regex.Match(value, @"post\-([0-9]+)$");
            if (!postIdMatch.Success)
                return null;

            return postIdMatch.Groups[1].Value;
        }

        private static string GetCategoryValue(string value)
        {
            var categoryMatch = Regex.Match(value, "kind#(post|comment)$");
            if(!categoryMatch.Success)
                return null;

            return categoryMatch.Groups[1].Value;
        }
    }
}
