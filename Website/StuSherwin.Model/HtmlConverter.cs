using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.IO;

namespace StuSherwin.Model
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
            var firstBrs = FindAllFirstNodesFromConsecutiveTags("br");
            if (firstBrs == null)
                return;

            foreach (var br in firstBrs)
            {
                WrapPrecedingNodeInTag(br, "p");
                if (!brsToRemove.Contains(br))
                    brsToRemove.Add(br);
                if (!brsToRemove.Contains(br.NextSibling))
                    brsToRemove.Add(br.NextSibling);
            }
        }

        private void WrapNextTextOfConsecutiveBrs(List<HtmlNode> brsToRemove)
        {
            var lastBrs = FindAllLastNodesFromConsecutiveTags("br");
            if (lastBrs == null)
                return;

            foreach (var br in lastBrs)
            {
                WrapNextNodeInTag(br, "p");
                if (!brsToRemove.Contains(br))
                    brsToRemove.Add(br);
                if (!brsToRemove.Contains(br.PreviousSibling))
                    brsToRemove.Add(br.PreviousSibling);
            }
        }

        private void WrapPrecedingNodeInTag(HtmlNode node, string wrappingTagName)
        {
            if (node.PreviousSibling.Name != wrappingTagName)
            {
                var wrappingTag = _htmlDocument.CreateElement(wrappingTagName);
                wrappingTag.InnerHtml = node.PreviousSibling.OuterHtml;
                ReplaceNode(node.PreviousSibling, wrappingTag);
            }
        }

        private void WrapNextNodeInTag(HtmlNode node, string wrappingTagName)
        {
            if (node.NextSibling.Name != wrappingTagName)
            {
                var wrappingTag = _htmlDocument.CreateElement(wrappingTagName);
                wrappingTag.InnerHtml = node.NextSibling.OuterHtml;
                ReplaceNode(node.NextSibling, wrappingTag);
            }
        }

        private IEnumerable<HtmlNode> FindAllFirstNodesFromConsecutiveTags(string tagName)
        {
            return _htmlDocument.DocumentNode
                .SelectNodes("//" + tagName)
                .Where(n =>
                    n.NextSibling != null
                    && n.NextSibling.Name == tagName
                    && (n.PreviousSibling == null || n.PreviousSibling.Name != tagName));
        }

        private IEnumerable<HtmlNode> FindAllLastNodesFromConsecutiveTags(string tagName)
        {
            return _htmlDocument.DocumentNode
                .SelectNodes("//" + tagName)
                .Where(n =>
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
    }
}
