using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    [MetadataType(typeof(Metadata.Post))]
    public partial class Post
    { }
}

namespace Website.Models.Metadata
{
    public class Post
    {
        [UIHint("HtmlContent")]
        public string Body { get; set; }
    }

}