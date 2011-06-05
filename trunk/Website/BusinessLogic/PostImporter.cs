using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using StuSherwin.Poco;

namespace BusinessLogic
{
    public class PostImporter
    {
        private Stream _inputFileStream;

        public PostImporter(Stream inputFileStream)
        {
            _inputFileStream = inputFileStream;
        }

        public IEnumerable<Post> GetPosts()
        {
            XDocument document = XDocument.Load(_inputFileStream);
            var elements = 
                from d in document.GetDescendants("entry")
                let idMatch = Regex.Match(d.GetElement("id").Value, @"post\-([0-9]+)$")
                where idMatch.Success
                let category = Regex.Match(d.GetElement("category").GetAttribute("term").Value, "kind#(post|comment)$").Groups[1].Value
                select new
                {
                    Id = idMatch.Groups[1].Value,
                    Published = DateTime.Parse(d.GetElement("published").Value),
                    Updated = DateTime.Parse(d.GetElement("updated").Value),
                    Category = category,
                    Title = d.GetElement("title").Value,
                    Content = d.GetElement("content").Value,
                    PostId = GetPostId(d)
                };

            List<Post> posts = new List<Post>();

            foreach (var postElement in elements
                .Where(e => e.Category == "post"))
            {
                var post = new Post
                {
                    Title = postElement.Title,
                    Body = postElement.Content,
                    Published = postElement.Published,
                    Updated = postElement.Updated
                };


                var comments = elements
                    .Where(e => e.Category == "comment" && e.PostId == postElement.Id)
                    .Select(e => new Comment
                    {
                        Title = e.Title,
                        Body = e.Content,
                        Date = e.Published,
                        Post = post
                    })
                    .ToArray();

                post.Comments = comments;

                posts.Add(post);
            }

            return posts;
        }

        private string GetPostId(XElement element)
        {
            var inReplyToElement = element.GetElement("in-reply-to");
            if(inReplyToElement == null)
                return null;

            var postIdMatch = Regex.Match(inReplyToElement.GetAttribute("ref").Value, @"post\-([0-9]+)$");
            if (!postIdMatch.Success)
                return null;

            return postIdMatch.Groups[1].Value;
        }
    }
}
