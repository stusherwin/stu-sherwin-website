﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;

namespace StuSherwin.Domain.Importing
{
    public class HtmlConverter
    {
        private HtmlDocument _htmlDocument;

        public HtmlConverter()
        {
        }

        public void LoadHtml(string html)
        {
            var doc = new HtmlDocument();
            HtmlNode.ElementsFlags["br"] = HtmlElementFlag.Empty;
            doc.OptionWriteEmptyNodes = true;
            doc.LoadHtml(html);
            if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
            {
                throw new HtmlParseException("Could not parse the HTML");
            }
            _htmlDocument = doc;
        }

        public string GetConvertedHtml()
        {
            var html = new StringBuilder();
            var writer = new StringWriter(html);
            _htmlDocument.Save(writer);

            // self-closed <br /> tags get mangled by InnerHtml property.
            var htmlWithFixedBreakTags = html.ToString().Replace("<br>", "<br />");

            return htmlWithFixedBreakTags;
        }

        public void ConvertHtml()
        {
            ConvertBoldSpanTagsToStrongTags();
            ConvertItalicSpanTagsToEmTags();
            ConvertItalicAndBoldSpanTagsToStrongEmTags();
            RemoveSpanTagsKeepingInnerHtml();
            ConvertDoubleBrTagsToParagraphTags();
        }

        public int GetNumberOfTagsOfType(string tagType)
        {
            return _htmlDocument.DocumentNode.SelectNodes("//" + tagType).Count();
        }

        public void ConvertBoldSpanTagsToStrongTags()
        {
            var spans = _htmlDocument.DocumentNode.SelectNodes("//span[@style='font-weight: bold;']");
            if (spans == null)
                return;

            foreach (var span in spans)
            {
                var strong = _htmlDocument.CreateElement("strong");
                strong.InnerHtml = span.InnerHtml;
                ReplaceNode(span, strong);
            }
        }

        public void ConvertItalicSpanTagsToEmTags()
        {
            var spans = _htmlDocument.DocumentNode.SelectNodes("//span[@style='font-style: italic;']");
            if (spans == null)
                return;

            foreach (var span in spans)
            {
                var em = _htmlDocument.CreateElement("em");
                em.InnerHtml = span.InnerHtml;
                ReplaceNode(span, em);
            }
        }

        public void ConvertItalicAndBoldSpanTagsToStrongEmTags()
        {
            var spans = _htmlDocument.DocumentNode.SelectNodes("//span[@style='font-weight: bold; font-style: italic;']");
            if (spans == null)
                return;

            foreach (var span in spans)
            {
                var em = _htmlDocument.CreateElement("em");
                em.InnerHtml = span.InnerHtml;
                var strong = _htmlDocument.CreateElement("strong");
                strong.AppendChild(em);
                ReplaceNode(span, strong);
            }
        }

        public void RemoveSpanTagsKeepingInnerHtml()
        {
            var spans = _htmlDocument.DocumentNode.SelectNodes("//span");
            if (spans == null)
                return;

            foreach (var span in spans)
            {
                var text = _htmlDocument.CreateTextNode(span.InnerHtml);
                ReplaceNode(span, text);
            }
        }

        public void ConvertDoubleBrTagsToParagraphTags()
        {
            List<HtmlNode> brsToRemove = new List<HtmlNode>();

            WrapPreviousTextOfConsecutiveBrs(brsToRemove);
            WrapNextTextOfConsecutiveBrs(brsToRemove);

            foreach (var br in brsToRemove)
            {
                br.ParentNode.RemoveChild(br);
            }
        }

        private void WrapPreviousTextOfConsecutiveBrs(List<HtmlNode> brsToRemove)
        {
            var firstBrs = FindAllFirstNodesFromConsecutiveTags("br").ToArray();
            if (firstBrs == null)
                return;

            for (int i = 0; i < firstBrs.Length; i++)
            {
                var br = firstBrs[i];
                var previousBr = i > 0
                    ? firstBrs[i - 1].NextSibling
                    : null;

                if (!brsToRemove.Contains(br))
                    brsToRemove.Add(br);
                if (!brsToRemove.Contains(br.NextSibling))
                    brsToRemove.Add(br.NextSibling);

                WrapPrecedingNodesInTag(br, previousBr, "p");
            }
        }

        private void WrapNextTextOfConsecutiveBrs(List<HtmlNode> brsToRemove)
        {
            var lastBrs = FindAllLastNodesFromConsecutiveTags("br").ToArray();
            if (lastBrs == null)
                return;

            for (int i = 0; i < lastBrs.Length; i++)
            {
                var br = lastBrs[i];
                var nextBr = i < lastBrs.Length - 1
                    ? lastBrs[i + 1].PreviousSibling
                    : null;

                if (!brsToRemove.Contains(br))
                    brsToRemove.Add(br);
                if (!brsToRemove.Contains(br.PreviousSibling))
                    brsToRemove.Add(br.PreviousSibling);

                WrapNextNodesInTag(br, nextBr, "p");
            }
        }

        private void WrapPrecedingNodesInTag(HtmlNode node, HtmlNode untilNode, string wrappingTagName)
        {
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Backwards);
            HtmlNode previousTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(node, untilNode);
            HtmlNode currentNode = node;
            while(previousTopLevelNode != null) {
                WrapPreviousSiblingsInTag(nodeWalker, currentNode, previousTopLevelNode, wrappingTagName);
                currentNode = previousTopLevelNode;
                previousTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(previousTopLevelNode, untilNode);
            }
            WrapPreviousSiblingsInTag(nodeWalker, currentNode, untilNode, wrappingTagName);
        }

        private void WrapPreviousSiblingsInTag(HtmlNodeWalker nodeWalker, HtmlNode node, HtmlNode untilNode, string wrappingTagName)
        {
            var previousNodes = nodeWalker.GetAllNextSiblingsUntilNode(node, untilNode);
            var wrappingTag = _htmlDocument.CreateElement(wrappingTagName);
            wrappingTag.InnerHtml = previousNodes.ToString();
            ReplaceNodes(previousNodes, wrappingTag);
        }

        private void WrapNextNodesInTag(HtmlNode node, HtmlNode untilNode, string wrappingTagName)
        {
            var nodeWalker = new HtmlNodeWalker(HtmlNodeWalkerDirection.Forwards);
            HtmlNode nextTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(node, untilNode);
            HtmlNode currentNode = node;
            while (nextTopLevelNode != null)
            {
                WrapNextSiblingsInTag(nodeWalker, currentNode, nextTopLevelNode, wrappingTagName);
                currentNode = nextTopLevelNode;
                nextTopLevelNode = nodeWalker.GetNextTopLevelSiblingBefore(nextTopLevelNode, untilNode);
            }
            WrapNextSiblingsInTag(nodeWalker, currentNode, untilNode, wrappingTagName);
        }

        private void WrapNextSiblingsInTag(HtmlNodeWalker nodeWalker, HtmlNode node, HtmlNode untilNode, string wrappingTagName)
        {
            var nextNodes = nodeWalker.GetAllNextSiblingsUntilNode(node, untilNode);
            var wrappingTag = _htmlDocument.CreateElement(wrappingTagName);
            wrappingTag.InnerHtml = nextNodes.ToString();
            ReplaceNodes(nextNodes, wrappingTag);
        }

        private IEnumerable<HtmlNode> FindAllFirstNodesFromConsecutiveTags(string tagName)
        {
            IEnumerable<HtmlNode> nodes = _htmlDocument.DocumentNode
                    .SelectNodes("//" + tagName)
                    .AsEnumerable()
                ?? new HtmlNode[] {};

            return nodes.Where(n =>
                n.NextSibling != null
                && n.NextSibling.Name == tagName
                && (n.PreviousSibling == null || n.PreviousSibling.Name != tagName));
        }

        private IEnumerable<HtmlNode> FindAllLastNodesFromConsecutiveTags(string tagName)
        {
            IEnumerable<HtmlNode> nodes = _htmlDocument.DocumentNode
                    .SelectNodes("//" + tagName)
                    .AsEnumerable()
                ?? new HtmlNode[] {};

            return nodes.Where(n =>
                n.PreviousSibling != null
                && n.PreviousSibling.Name == tagName
                && (n.NextSibling == null || n.NextSibling.Name != tagName));
        }

        private void ReplaceNode(HtmlNode originalNode, HtmlNode replacementNode)
        {
            var parentNode = originalNode.ParentNode;
            parentNode.InsertAfter(replacementNode, originalNode);
            parentNode.RemoveChild(originalNode, false);
        }

        private void ReplaceNodes(IEnumerable<HtmlNode> originalNodes, HtmlNode replacementNode)
        {
            if (originalNodes.Count() == 0)
                return;

            var parentNode = originalNodes.First().ParentNode;
            parentNode.InsertAfter(replacementNode, originalNodes.Last());
            foreach (var node in originalNodes)
            {
                parentNode.RemoveChild(node, false);
            }
        }
    }
}
