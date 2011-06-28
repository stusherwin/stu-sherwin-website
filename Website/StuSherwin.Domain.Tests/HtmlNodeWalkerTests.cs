using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using HtmlAgilityPack;
using StuSherwin.Domain.Importing;

namespace StuSherwin.Domain.Tests
{
    public class HtmlNodeWalkerTests
    {
        [Fact]
        public void GetAllNextSiblings_when_direction_is_backwards_returns_all_previous_siblings()
        {
            string html = "<p>Node 1</p><p>Node 2</p><p>Node 3</p><div>Current Node</div>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//div");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var previousNodes = nodeWalker.GetAllNextSiblings(currentNode);
            Assert.Equal(3, previousNodes.Count);
            Assert.Equal("<p>Node 1</p>", previousNodes[0].OuterHtml);
            Assert.Equal("<p>Node 2</p>", previousNodes[1].OuterHtml);
            Assert.Equal("<p>Node 3</p>", previousNodes[2].OuterHtml);
        }

        [Fact]
        public void GetAllNextSiblings_when_direction_is_forwards_returns_all_next_siblings()
        {
            string html = "<div>Current Node</div><p>Node 1</p><p>Node 2</p><p>Node 3</p>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//div");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var previousNodes = nodeWalker.GetAllNextSiblings(currentNode);
            Assert.Equal(3, previousNodes.Count);
            Assert.Equal("<p>Node 1</p>", previousNodes[0].OuterHtml);
            Assert.Equal("<p>Node 2</p>", previousNodes[1].OuterHtml);
            Assert.Equal("<p>Node 3</p>", previousNodes[2].OuterHtml);
        }

        [Fact]
        public void GetAllNextSiblingsUntilNode_when_direction_is_backwards_returns_all_previous_siblings_between_the_current_node_and_the_specified_node()
        {
            string html = "<p>Node 1</p><br /><p>Node 2</p><p>Node 3</p><p>Node 4</p><div>Current Node</div>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//div");
            HtmlNode untilNode = htmlDoc.DocumentNode.SelectSingleNode("//br");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var previousNodes = nodeWalker.GetAllNextSiblingsUntilNode(currentNode, untilNode);
            Assert.Equal(3, previousNodes.Count);
            Assert.Equal("<p>Node 2</p>", previousNodes[0].OuterHtml);
            Assert.Equal("<p>Node 3</p>", previousNodes[1].OuterHtml);
            Assert.Equal("<p>Node 4</p>", previousNodes[2].OuterHtml);
        }

        [Fact]
        public void GetAllNextSiblingsUntilNode_when_direction_is_backwards_when_specified_node_is_null_then_returns_all_previous_siblings()
        {
            string html = "<p>Node 1</p><br /><p>Node 2</p><p>Node 3</p><p>Node 4</p><div>Current Node</div>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//div");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var previousNodes = nodeWalker.GetAllNextSiblingsUntilNode(currentNode, null);
            Assert.Equal(5, previousNodes.Count);
            Assert.Equal("<p>Node 1</p>", previousNodes[0].OuterHtml);
            Assert.Equal("<br />", previousNodes[1].OuterHtml);
            Assert.Equal("<p>Node 2</p>", previousNodes[2].OuterHtml);
            Assert.Equal("<p>Node 3</p>", previousNodes[3].OuterHtml);
            Assert.Equal("<p>Node 4</p>", previousNodes[4].OuterHtml);
        }

        [Fact]
        public void GetAllNextSiblingsUntilNode_when_direction_is_forwards_returns_all_next_siblings_between_the_current_node_and_the_specified_node()
        {
            string html = "<div>Current Node</div><p>Node 1</p><p>Node 2</p><p>Node 3</p><br /><p>Node 4</p>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//div");
            HtmlNode untilNode = htmlDoc.DocumentNode.SelectSingleNode("//br");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var nextNodes = nodeWalker.GetAllNextSiblingsUntilNode(currentNode, untilNode);
            Assert.Equal(3, nextNodes.Count);
            Assert.Equal("<p>Node 1</p>", nextNodes[0].OuterHtml);
            Assert.Equal("<p>Node 2</p>", nextNodes[1].OuterHtml);
            Assert.Equal("<p>Node 3</p>", nextNodes[2].OuterHtml);
        }

        [Fact]
        public void GetAllNextSiblingsUntilNode_when_direction_is_forwards_when_specified_node_is_null_then_returns_all_next_siblings()
        {
            string html = "<div>Current Node</div><p>Node 1</p><p>Node 2</p><p>Node 3</p><br /><p>Node 4</p>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//div");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var nextNodes = nodeWalker.GetAllNextSiblingsUntilNode(currentNode, null);
            Assert.Equal(5, nextNodes.Count);
            Assert.Equal("<p>Node 1</p>", nextNodes[0].OuterHtml);
            Assert.Equal("<p>Node 2</p>", nextNodes[1].OuterHtml);
            Assert.Equal("<p>Node 3</p>", nextNodes[2].OuterHtml);
            Assert.Equal("<br />", nextNodes[3].OuterHtml);
            Assert.Equal("<p>Node 4</p>", nextNodes[4].OuterHtml);
        }

        [Fact]
        public void GetAllNonTopLevelNextSiblings_when_direction_is_forwards_returns_all_next_siblings_that_are_not_top_level_nodes()
        {
            string html = "<em>Current Node</em><div>Node 1</div><div>Node 2</div><p>Node 3</p>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var nextNodes = nodeWalker.GetAllNonTopLevelNextSiblings(currentNode);
            Assert.Equal(2, nextNodes.Count);
            Assert.Equal("<div>Node 1</div>", nextNodes[0].OuterHtml);
            Assert.Equal("<div>Node 2</div>", nextNodes[1].OuterHtml);
        }

        [Fact]
        public void GetAllNonTopLevelNextSiblings_when_direction_is_forwards_with_no_top_level_nodes_returns_all_next_siblings()
        {
            string html = "<em>Current Node</em><div>Node 1</div><div>Node 2</div><div>Node 3</div>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var nextNodes = nodeWalker.GetAllNonTopLevelNextSiblings(currentNode);
            Assert.Equal(3, nextNodes.Count);
            Assert.Equal("<div>Node 1</div>", nextNodes[0].OuterHtml);
            Assert.Equal("<div>Node 2</div>", nextNodes[1].OuterHtml);
            Assert.Equal("<div>Node 3</div>", nextNodes[2].OuterHtml);
        }

        [Fact]
        public void GetAllNonTopLevelNextSiblings_when_direction_is_backwards_returns_all_previous_siblings_that_are_not_top_level_nodes()
        {
            string html = "<p>Node 1</p><div>Node 2</div><div>Node 3</div><em>Current Node</em>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var nextNodes = nodeWalker.GetAllNonTopLevelNextSiblings(currentNode);
            Assert.Equal(2, nextNodes.Count);
            Assert.Equal("<div>Node 2</div>", nextNodes[0].OuterHtml);
            Assert.Equal("<div>Node 3</div>", nextNodes[1].OuterHtml);
        }

        [Fact]
        public void GetAllNonTopLevelNextSiblings_when_direction_is_backwards_with_no_top_level_nodes_returns_all_previous_siblings()
        {
            string html = "<div>Node 1</div><div>Node 2</div><div>Node 3</div><em>Current Node</em>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var nextNodes = nodeWalker.GetAllNonTopLevelNextSiblings(currentNode);
            Assert.Equal(3, nextNodes.Count);
            Assert.Equal("<div>Node 1</div>", nextNodes[0].OuterHtml);
            Assert.Equal("<div>Node 2</div>", nextNodes[1].OuterHtml);
            Assert.Equal("<div>Node 3</div>", nextNodes[2].OuterHtml);
        }

        [Fact]
        public void GetNextTopLevelSibling_when_direction_is_forwards_returns_next_top_level_node()
        {
            string html = "<em>Current Node</em><div>Node 1</div><div>Node 2</div><p>Node 3</p><div>Node 4</div><blockquote>Blockquote</blockquote>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var nextTopLevelNode = nodeWalker.GetNextTopLevelSibling(currentNode);
            Assert.Equal("<p>Node 3</p>", nextTopLevelNode.OuterHtml);
            nextTopLevelNode = nodeWalker.GetNextTopLevelSibling(nextTopLevelNode);
            Assert.Equal("<blockquote>Blockquote</blockquote>", nextTopLevelNode.OuterHtml);
        }

        [Fact]
        public void GetNextTopLevelSibling_when_direction_is_forwards_with_no_top_level_nodes_returns_null()
        {
            string html = "<em>Current Node</em><div>Node 1</div><div>Node 2</div><div>Node 3</div>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var nextTopLevelNode = nodeWalker.GetNextTopLevelSibling(currentNode);
            Assert.Null(nextTopLevelNode);
        }

        [Fact]
        public void GetNextTopLevelSibling_when_direction_is_backwards_returns_previous_top_level_node()
        {
            string html = "<div>Node 1</div><blockquote>Blockquote</blockquote><div>Node 2</div><p>Node 3</p><div>Node 4</div><em>Current Node</em>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var previousTopLevelNode = nodeWalker.GetNextTopLevelSibling(currentNode);
            Assert.Equal("<p>Node 3</p>", previousTopLevelNode.OuterHtml);
            previousTopLevelNode = nodeWalker.GetNextTopLevelSibling(previousTopLevelNode);
            Assert.Equal("<blockquote>Blockquote</blockquote>", previousTopLevelNode.OuterHtml);
        }

        [Fact]
        public void GetNextTopLevelSiblings_when_direction_is_backwards_with_no_top_level_nodes_returns_null()
        {
            string html = "<div>Node 1</div><div>Node 2</div><div>Node 3</div><em>Current Node</em>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var previousTopLevelNode = nodeWalker.GetNextTopLevelSibling(currentNode);
            Assert.Null(previousTopLevelNode);
        }

        [Fact]
        public void GetNextTopLevelSiblingBefore_when_direction_is_forwards_returns_next_top_level_node_before_specified_node()
        {
            string html = "<em>Current Node</em><div>Node 1</div><div>Node 2</div><p>Node 3</p><p>Node 4</p><strong>Stop</strong><p>Node 5</p>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            HtmlNode stopNode = htmlDoc.DocumentNode.SelectSingleNode("//strong");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var nextTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(currentNode, stopNode);
            Assert.Equal("<p>Node 3</p>", nextTopLevelNode.OuterHtml);
            nextTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(nextTopLevelNode, stopNode);
            Assert.Equal("<p>Node 4</p>", nextTopLevelNode.OuterHtml);
            nextTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(nextTopLevelNode, stopNode);
            Assert.Null(nextTopLevelNode);
        }

        [Fact]
        public void GetNextTopLevelSiblingBefore_when_direction_is_forwards_with_no_top_level_nodes_returns_null()
        {
            string html = "<em>Current Node</em><div>Node 1</div><div>Node 2</div><div>Node 3</div><strong>Stop</strong>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            HtmlNode stopNode = htmlDoc.DocumentNode.SelectSingleNode("//strong");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var nextTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(currentNode, stopNode);
            Assert.Null(nextTopLevelNode);
        }

        [Fact]
        public void GetNextTopLevelSiblingBefore_when_direction_is_forwards_when_stop_node_equals_current_node_returns_null()
        {
            string html = "<em>Current Node</em><div>Node 1</div><div>Node 2</div><p>Node 3</p><p>Node 4</p><strong>Stop</strong><p>Node 5</p>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            var nextTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(currentNode, currentNode);
            Assert.Null(nextTopLevelNode);
        }

        [Fact]
        public void GetNextTopLevelSiblingBefore_when_direction_is_backwards_returns_previous_top_level_node_before_specified_node()
        {
            string html = "<div>Node 1</div><p>Node 2</p><strong>Stop</strong><p>Node 3</p><p>Node 4</p><div>Node 5</div><em>Current Node</em>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            HtmlNode stopNode = htmlDoc.DocumentNode.SelectSingleNode("//strong");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var previousTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(currentNode, stopNode);
            Assert.Equal("<p>Node 4</p>", previousTopLevelNode.OuterHtml);
            previousTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(previousTopLevelNode, stopNode);
            Assert.Equal("<p>Node 3</p>", previousTopLevelNode.OuterHtml);
            previousTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(previousTopLevelNode, stopNode);
            Assert.Null(previousTopLevelNode);
        }

        [Fact]
        public void GetNextTopLevelSiblingBefore_when_direction_is_backwards_with_no_top_level_nodes_returns_null()
        {
            string html = "<strong>Stop</strong><div>Node 1</div><div>Node 2</div><div>Node 3</div><em>Current Node</em>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            HtmlNode stopNode = htmlDoc.DocumentNode.SelectSingleNode("//strong");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var previousTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(currentNode, stopNode);
            Assert.Null(previousTopLevelNode);
        }

        [Fact]
        public void GetNextTopLevelSiblingBefore_when_direction_is_backwards_when_stop_node_equals_current_node_returns_null()
        {
            string html = "<div>Node 1</div><div>Node 2</div><p>Node 3</p><p>Node 4</p><strong>Stop</strong><p>Node 5</p><em>Current Node</em>";
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            HtmlNode currentNode = htmlDoc.DocumentNode.SelectSingleNode("//em");
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            var nextTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(currentNode, currentNode);
            Assert.Null(nextTopLevelNode);
        }
    }
}
