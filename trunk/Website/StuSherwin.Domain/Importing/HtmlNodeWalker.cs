using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace StuSherwin.Domain.Importing
{
    public class HtmlNodeWalker
    {
        private string[] _topLevelTags = { "p", "blockquote", "ul", "ol" };

        public HtmlNodeWalker()
        {
        }

        public List<HtmlNode> GetAllPreviousSiblings(HtmlNode currentNode)
        {
            var previousSiblings = new List<HtmlNode>();
            HtmlNode node = currentNode.PreviousSibling;
            while (node != null)
            {
                previousSiblings.Insert(0, node);
                node = node.PreviousSibling;
            }
            return previousSiblings;
        }

        public List<HtmlNode> GetAllNextSiblings(HtmlNode currentNode)
        {
            var nextSiblings = new List<HtmlNode>();
            HtmlNode node = currentNode.NextSibling;
            while (node != null)
            {
                nextSiblings.Add(node);
                node = node.NextSibling;
            }
            return nextSiblings;
        }

        public List<HtmlNode> GetAllPreviousSiblingsUntilNode(HtmlNode currentNode, HtmlNode untilNode)
        {
            var previousSiblings = new List<HtmlNode>();
            HtmlNode node = currentNode.PreviousSibling;
            while (node != null && node != untilNode)
            {
                previousSiblings.Insert(0, node);
                node = node.PreviousSibling;
            }
            return previousSiblings;
        }

        public List<HtmlNode> GetAllNextSiblingsUntilNode(HtmlNode currentNode, HtmlNode untilNode)
        {
            var nextSiblings = new List<HtmlNode>();
            HtmlNode node = currentNode.NextSibling;
            while (node != null && node != untilNode)
            {
                nextSiblings.Add(node);
                node = node.NextSibling;
            }
            return nextSiblings;
        }

        public List<HtmlNode> GetAllNonTopLevelNextSiblings(HtmlNode currentNode)
        {
            var nextSiblings = new List<HtmlNode>();
            HtmlNode node = currentNode.NextSibling;
            while (node != null && !_topLevelTags.Contains(node.Name))
            {
                nextSiblings.Add(node);
                node = node.NextSibling;
            }
            return nextSiblings;
        }

        public List<HtmlNode> GetAllNonTopLevelPreviousSiblings(HtmlNode currentNode)
        {
            var previousSiblings = new List<HtmlNode>();
            HtmlNode node = currentNode.PreviousSibling;
            while (node != null && !_topLevelTags.Contains(node.Name))
            {
                previousSiblings.Insert(0, node);
                node = node.PreviousSibling;
            }
            return previousSiblings;
        }

        public HtmlNode GetNextTopLevelSibling(HtmlNode currentNode)
        {
            HtmlNode node = currentNode.NextSibling;

            while (node != null && !_topLevelTags.Contains(node.Name))
            {
                node = node.NextSibling;
            }

            if (node == null || !_topLevelTags.Contains(node.Name))
                return null;

            return node;
        }

        public HtmlNode GetPreviousTopLevelSibling(HtmlNode currentNode)
        {
            HtmlNode node = currentNode.PreviousSibling;

            while (node != null && !_topLevelTags.Contains(node.Name))
            {
                node = node.PreviousSibling;
            }

            if (node == null || !_topLevelTags.Contains(node.Name))
                return null;

            return node;
        }

        public HtmlNode GetNextTopLevelSiblingBefore(HtmlNode currentNode, HtmlNode stopNode)
        {
            if (currentNode == stopNode)
                return null;

            HtmlNode node = currentNode.NextSibling;

            while (node != null && !_topLevelTags.Contains(node.Name) && node != stopNode)
            {
                node = node.NextSibling;
            }

            if (node == null || !_topLevelTags.Contains(node.Name) || node == stopNode)
                return null;
            
            return node;
        }

        public HtmlNode GetPreviousTopLevelSiblingBefore(HtmlNode currentNode, HtmlNode stopNode)
        {
            if (currentNode == stopNode)
                return null;

            HtmlNode node = currentNode.PreviousSibling;
            while (node != null && !_topLevelTags.Contains(node.Name) && node != stopNode)
            {
                node = node.PreviousSibling;
            }

            if (node == null || !_topLevelTags.Contains(node.Name) || node == stopNode)
                return null;

            return node;
        }

        public string GetNodesHtml(IEnumerable<HtmlNode> nodes)
        {
            return nodes.Aggregate("", (a, n) => a += n.OuterHtml);
        }
    }
}
