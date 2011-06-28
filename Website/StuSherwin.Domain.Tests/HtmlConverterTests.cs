using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using StuSherwin.Domain.Importing;

namespace StuSherwin.Domain.Tests
{
    public class HtmlConverterTests
    {
        [Fact]
        public void HtmlDocumentLoaded()
        {
            var html = "<p>This is a test <span style=\"font-weight: bold;\">Some bold text</span></p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            Assert.Equal(html, converter.GetConvertedHtml());
        }

        [Fact]
        public void InvalidHtmlThrowsHtmlParseException()
        {
            var html = "<p>This is a test <span style=\"font-weight: bold;\">Some bold text</spat></>";
            var converter = new HtmlConverter();
            var exception = Assert.Throws<HtmlParseException>(() =>
                {
                    converter.LoadHtml(html);
                });
            Assert.Equal(exception.Message, "Could not parse the HTML");
        }

        [Fact]
        public void CountOfTagsOfSpecificTypeIsCorrect()
        {
            var html = "<p><span>span</span><b>b1</b><b>b2</b><b>b3</b><b>b4</b></p><p>p2</p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            Assert.Equal(1, converter.GetNumberOfTagsOfType("span"));
            Assert.Equal(4, converter.GetNumberOfTagsOfType("b"));
            Assert.Equal(2, converter.GetNumberOfTagsOfType("p"));
        }

        [Fact]
        public void BoldSpanTagsConvertedToStrongTags()
        {
            var html = "<p>This is a test <span style=\"font-weight: bold;\">Some bold text</span><span style=\"font-weight: bold;\">Some more bold text</span></p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertBoldSpanTagsToStrongTags();
            var expectedHtml = "<p>This is a test <strong>Some bold text</strong><strong>Some more bold text</strong></p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void BoldSpanTagsConvertedToStrongTagsRetainingChildNodes()
        {
            var html = "<p>This is a test <span style=\"font-weight: bold;\">Some bold text with a <a href=\"#\"><b>bold link</b></a><br /></span></p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertBoldSpanTagsToStrongTags();
            var expectedHtml = "<p>This is a test <strong>Some bold text with a <a href=\"#\"><b>bold link</b></a><br /></strong></p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void NonBoldSpanTagsNotConvertedToStrongTags()
        {
            var html = "<p>This is a test <span style=\"font-weight: bold;\">Some bold text</span><span>Some more bold text</span></p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertBoldSpanTagsToStrongTags();
            var expectedHtml = "<p>This is a test <strong>Some bold text</strong><span>Some more bold text</span></p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void ItalicSpanTagsConvertedToEmTags()
        {
            var html = "<p>This is a test <span style=\"font-style: italic;\">Some italic text</span><span style=\"font-style: italic;\">Some more italic text</span></p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertItalicSpanTagsToEmTags();
            var expectedHtml = "<p>This is a test <em>Some italic text</em><em>Some more italic text</em></p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void ItalicSpanTagsConvertedToEmTagsRetainingChildNodes()
        {
            var html = "<p>This is a test <span style=\"font-style: italic;\">Some italic text with a <a href=\"#\"><b>bold link</b></a><br /></span></p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertItalicSpanTagsToEmTags();
            var expectedHtml = "<p>This is a test <em>Some italic text with a <a href=\"#\"><b>bold link</b></a><br /></em></p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void NonItalicSpanTagsNotConvertedToEmTags()
        {
            var html = "<p>This is a test <span style=\"font-style: italic;\">Some italic text</span><span>Some non italic text</span></p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertItalicSpanTagsToEmTags();
            var expectedHtml = "<p>This is a test <em>Some italic text</em><span>Some non italic text</span></p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void ItalicAndBoldSpanTagsConvertedToStrongEmTags()
        {
            var html = "<p>This is a test <span style=\"font-weight: bold; font-style: italic;\">Some bold and italic text</span><span style=\"font-weight: bold; font-style: italic;\">Some more bold and italic text</span></p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertItalicAndBoldSpanTagsToStrongEmTags();
            var expectedHtml = "<p>This is a test <strong><em>Some bold and italic text</em></strong><strong><em>Some more bold and italic text</em></strong></p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void RemoveSpanTags_removes_span_tags_keeping_inner_html()
        {
            var html = "<p>This is a test <span style=\"font-weight: bold; font-style: italic;\">Some bold and italic text</span><span style=\"font-weight: bold; font-style: italic;\">Some more bold and italic text</span></p>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.RemoveSpanTagsKeepingInnerHtml();
            var expectedHtml = "<p>This is a test Some bold and italic textSome more bold and italic text</p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void DoubleBrTagsRemovedAndPreviousAndNextTextWrappedInParagraphTags()
        {
            var html = "This is some text.<br /><br />This is some more text.<br /><br />Even more text.";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertDoubleBrTagsToParagraphTags();
            var expectedHtml = "<p>This is some text.</p><p>This is some more text.</p><p>Even more text.</p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void DoubleBrTagsRemovedIgnoringExistingParagraphTags()
        {
            var html = "This is some text.<br /><br /><p>This is some more text.</p><br /><br />Some more text.";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertDoubleBrTagsToParagraphTags();
            var expectedHtml = "<p>This is some text.</p><p>This is some more text.</p><p>Some more text.</p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void DoubleBrTagsRemovedIgnoringExtraBrTags()
        {
            var html = "This is some text.<br />This is some more text.<br /><br /><br /><br /><br />Some more text.<br />";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertDoubleBrTagsToParagraphTags();
            var expectedHtml = "<p>This is some text.<br />This is some more text.</p><br /><p>Some more text.<br /></p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void DoubleBrTagsRemovedWrappingOtherTags()
        {
            var html = "This is some text<a href=\"#\">link</a>.<br />This is some more <strong>text</strong>.<br /><br /><br /><br /><br />Some more text.<br />";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertDoubleBrTagsToParagraphTags();
            var expectedHtml = "<p>This is some text<a href=\"#\">link</a>.<br />This is some more <strong>text</strong>.</p><br /><p>Some more text.<br /></p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void DoubleBrTagsRemovedNotWrappingBlockLevelTags()
        {
            var html = "This is some text.<br /><br />This is some more text.<blockquote>This is a blockquote</blockquote><br /><br />Some more text.<ul><li>List item</li></ul>Some more text.";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertDoubleBrTagsToParagraphTags();
            var expectedHtml = "<p>This is some text.</p><p>This is some more text.</p><blockquote>This is a blockquote</blockquote><p>Some more text.</p><ul><li>List item</li></ul><p>Some more text.</p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }

        [Fact]
        public void ConvertDoubleBrTagsToParagraphTags_when_no_double_br_tags_does_nothing()
        {
            var html = "This is some text. <br />This is some more text.<blockquote>This is a blockquote</blockquote>Some more text.<ul><li>List item</li></ul>Some more text.";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.ConvertDoubleBrTagsToParagraphTags();
            Assert.Equal(html, converter.GetConvertedHtml());
        }

        [Fact]
        public void RemoveSpanTags_followed_by_ConvertDoubleBrTagsToParagraphTags_removes_spans_and_converts_to_paragraphs()
        {
            var html = "This is a test <span style=\"font-weight: bold; font-style: italic;\">Some bold and italic text</span><blockquote>Blockquote</blockquote><span style=\"font-weight: bold; font-style: italic;\">Some more bold and italic text</span>";
            var converter = new HtmlConverter();
            converter.LoadHtml(html);
            converter.RemoveSpanTagsKeepingInnerHtml();
            converter.ConvertDoubleBrTagsToParagraphTags();
            var expectedHtml = "<p>This is a test Some bold and italic text</p><blockquote>Blockquote</blockquote><p>Some more bold and italic text</p>";
            Assert.Equal(expectedHtml, converter.GetConvertedHtml());
        }
    }
}
