using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace StuSherwin.Mvc.Core
{
    public static class HtmlExtensions
    {
        public static HelperResult ActionLinkIf(this HtmlHelper helper, bool condition, string text, string action, string controller, object routeValues)
        {
            return ActionLinkIf(helper, condition, text, action, controller, routeValues,
                s => new HelperResult(w => w.Write(s)),
                s => new HelperResult(w => w.Write(s)));
        }

        public static HelperResult ActionLinkIf(this HtmlHelper helper, bool condition, string text, string action, string controller, object routeValues, Func<MvcHtmlString, HelperResult> trueTemplate, Func<string, HelperResult> falseTemplate)
        {
            if (condition)
            {
                return new HelperResult(writer =>
                    trueTemplate(helper.ActionLink(text, action, controller, routeValues, null))
                            .WriteTo(writer));
            }
            else
            {
                return new HelperResult(writer =>
                    falseTemplate(text)
                            .WriteTo(writer));
            }
        }
    }
}