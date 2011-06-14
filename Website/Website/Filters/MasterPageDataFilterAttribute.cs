using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.Filters
{
    public class MasterPageDataFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Category = FindCategory(filterContext);
            filterContext.Controller.ViewBag.Page = FindPage(filterContext);
            filterContext.Controller.ViewBag.PostId = FindPostId(filterContext);
            base.OnActionExecuting(filterContext);
        }

        private string FindCategory(ActionExecutingContext filterContext)
        {
            string controller = GetRouteValue("controller", filterContext);

            if (controller == "Post")
            {
                return GetRouteValue("category", filterContext);
            }
            else if (IsPageController(controller))
            {
                return controller;
            }
             
            return null;
        }

        private string FindPage(ActionExecutingContext filterContext)
        {
            string controller = GetRouteValue("controller", filterContext);

            if (IsPageController(controller))
            {
                return GetRouteValue("action", filterContext);
            }

            return null;
        }

        private int? FindPostId(ActionExecutingContext filterContext)
        {
            string controller = GetRouteValue("controller", filterContext);

            if (controller == "Post")
            {
                int parsedPostId;
                string postIdString = GetRouteValue("id", filterContext);
                if (postIdString != null && int.TryParse(postIdString, out parsedPostId))
                    return parsedPostId;
            }

            return null;
        }

        private string GetRouteValue(string routeValueKey, ActionExecutingContext filterContext)
        {
            if(!filterContext.Controller.ControllerContext.RouteData.Values.ContainsKey(routeValueKey))
                return null;

            return filterContext.Controller.ControllerContext.RouteData.Values[routeValueKey].ToString();
        }

        private bool IsPageController(string controller)
        {
            return new[] { "Code", "Music", "Philosophy" }.Contains(controller);
        }
    }
}