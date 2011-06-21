using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using StuSherwin.Model;

namespace StuSherwin.Model
{
    public class PostImporter
    {
        public PostImporter()
        {
        }

        public IEnumerable<Post> ImportPosts(Stream inputFileStream, Category category)
        {
            XDocument document = XDocument.Load(inputFileStream);
            var entryElements = EntryElement.GetAllEntries(document);
            var posts = CreatePosts(entryElements, category);
            return posts;
        }

        private IEnumerable<Post> CreatePosts(IEnumerable<EntryElement> entryElements, Category category)
        {
            List<Post> posts = new List<Post>();

            foreach (var postElement in entryElements
                .Where(e => e.Category == "post"))
            {
                var post = new Post
                {
                    Title = postElement.Title,
                    Body = postElement.Content,
                    Published = postElement.Published,
                    Updated = postElement.Updated,
                    Category = category,
                    Code = CreateCodeFromTitle(postElement.Title),
                    OldUrl = postElement.PostUri
                };

                post.Comments = CreateCommentsForPost(entryElements, postElement.ElementId);

                posts.Add(post);
            }

            return posts;
        }

        private ICollection<Comment> CreateCommentsForPost(IEnumerable<EntryElement> entryElements, string postElementId)
        {
            var comments = entryElements
                .Where(e => e.Category == "comment" && e.PostElementId == postElementId)
                .Select(e => new Comment
                {
                    Title = e.Title,
                    Body = e.Content,
                    Date = e.Published,
                    Author = e.Author,
                    Website = e.AuthorUri
                });
            return comments.ToArray();
        }

        private string CreateCodeFromTitle(string title)
        {
            if (title == null)
                return null;

            return Regex.Replace(title.Replace(' ', '-'), "^[A-Za-z\\-0-9]", "");
        }
    }
}
