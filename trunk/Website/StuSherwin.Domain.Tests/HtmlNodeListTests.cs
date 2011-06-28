using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using HtmlAgilityPack;
using StuSherwin.Domain.Importing;

namespace StuSherwin.Domain.Tests
{
    public class HtmlNodeListTests
    {
        [Fact]
        public void HtmlNodeList_ToString_returns_OuterHtml_of_all_nodes_in_list()
        {
            string html = "<div>Current Node</div><p>Node 1</p><p>Node 2</p><p>Node 3</p><br /><p>Node 4</p>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var nodeList = new HtmlNodeList(HtmlNodeWalkerDirection.Forwards, htmlDoc.DocumentNode.SelectNodes("//p"));
            Assert.Equal("<p>Node 1</p><p>Node 2</p><p>Node 3</p><p>Node 4</p>", nodeList.ToString());
        }
    }
}
