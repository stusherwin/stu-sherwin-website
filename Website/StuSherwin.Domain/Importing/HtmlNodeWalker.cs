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

        public HtmlNodeWalkerDirection Direction { get; private set; }

        public HtmlNodeWalker(HtmlNodeWalkerDirection direction)
        {
            Direction = direction;
        }

        private HtmlNode GetNextSibling(HtmlNode node)
        {
            return Direction == HtmlNodeWalkerDirection.Forwards
                ? node.NextSibling
                : node.PreviousSibling;
        }

        private void AddNodeToList(HtmlNodeList list, HtmlNode node)
        {
            list.Add(node);
        }

        public HtmlNodeList GetAllNextSiblings(HtmlNode currentNode)
        {
            var nextSiblings = new HtmlNodeList(Direction);
            HtmlNode node = GetNextSibling(currentNode);
            while (node != null)
            {
                AddNodeToList(nextSiblings, node);
                node = GetNextSibling(node);
            }
            return nextSiblings;
        }

        public HtmlNodeList GetAllNextSiblingsUntilNode(HtmlNode currentNode, HtmlNode untilNode)
        {
            var nextSiblings = new HtmlNodeList(Direction);
            HtmlNode node = GetNextSibling(currentNode);
            while (node != null && node != untilNode)
            {
                AddNodeToList(nextSiblings, node);
                node = GetNextSibling(node);
            }
            return nextSiblings;
        }

        public HtmlNodeList GetAllNonTopLevelNextSiblings(HtmlNode currentNode)
        {
            var nextSiblings = new HtmlNodeList(Direction);
            HtmlNode node = GetNextSibling(currentNode);
            while (node != null && !_topLevelTags.Contains(node.Name))
            {
                AddNodeToList(nextSiblings, node);
                node = GetNextSibling(node);
            }
            return nextSiblings;
        }

        public HtmlNode GetNextTopLevelSibling(HtmlNode currentNode)
        {
            HtmlNode node = GetNextSibling(currentNode);

            while (node != null && !_topLevelTags.Contains(node.Name))
            {
                node = GetNextSibling(node);
            }

            if (node == null || !_topLevelTags.Contains(node.Name))
                return null;

            return node;
        }

        public HtmlNode GetNextTopLevelSiblingBefore(HtmlNode currentNode, HtmlNode stopNode)
        {
            if (currentNode == stopNode)
                return null;

            HtmlNode node = GetNextSibling(currentNode);

            while (node != null && !_topLevelTags.Contains(node.Name) && node != stopNode)
            {
                node = GetNextSibling(node);
            }

            if (node == null || !_topLevelTags.Contains(node.Name) || node == stopNode)
                return null;
            
            return node;
        }
    }
}
