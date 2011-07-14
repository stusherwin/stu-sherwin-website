using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StuSherwin.Mvc.Models;
using StuSherwin.Mvc.Models.Shared;

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

            if (model != null)
            {
                model.Layout = new Models.Shared.Layout
                {
                    Category = FindCategory(filterContext),
                    Page = FindPage(filterContext),
                    PostCode = FindPostCode(filterContext),
                    Type = FindLayoutType(filterContext)
                };
            }

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

        private string FindPostCode(ActionExecutedContext filterContext)
        {
            string controller = GetRouteValue("controller", filterContext);

            if (controller == "Post")
            {
                return GetRouteValue("code", filterContext);
            }

            return null;
        }

        private Layout.LayoutType FindLayoutType(ActionExecutedContext filterContext)
        {
            string controller = GetRouteValue("controller", filterContext);

            switch (controller)
            {
                case "Home":
                    return Layout.LayoutType.Home;
                case "Admin":
                case "Account":
                    return Layout.LayoutType.Admin;
                default:
                    return Layout.LayoutType.Post;
            }
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