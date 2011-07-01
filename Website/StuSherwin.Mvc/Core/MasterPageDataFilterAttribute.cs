using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StuSherwin.Mvc.Models;

namespace StuSherwin.Mvc.Core
{
    public class MasterPageDataFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Model model = filterContext.Controller.ViewData.Model as Model;

            if (model == null)
            {
                model = new Model();
                filterContext.Controller.ViewData.Model = model;
            }

            model.Layout = new Models.Shared.Layout
            {
                Category = FindCategory(filterContext),
                Page = FindPage(filterContext),
                PostId = FindPostId(filterContext),
                IsHomePage = IsHomePage(filterContext)
            };

            base.OnActionExecuted(filterContext);
        }

        private string FindCategory(ActionExecutedContext filterContext)
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

        private string FindPage(ActionExecutedContext filterContext)
        {
            string controller = GetRouteValue("controller", filterContext);

            if (IsPageController(controller))
            {
                return GetRouteValue("action", filterContext);
            }

            return null;
        }

        private int? FindPostId(ActionExecutedContext filterContext)
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

        private bool IsHomePage(ActionExecutedContext filterContext)
        {
            string controller = GetRouteValue("controller", filterContext);

            return controller == "Home";
        }

        private string GetRouteValue(string routeValueKey, ActionExecutedContext filterContext)
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