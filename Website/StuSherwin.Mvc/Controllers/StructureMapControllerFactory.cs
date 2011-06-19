using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using StructureMap;

namespace StuSherwin.Mvc
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        public StructureMapControllerFactory()
        {
        }

        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            IController controller = null;

            Type controllerType = GetControllerType(requestContext, controllerName);

            if (controllerType != null)
            {
                if (!typeof(IController).IsAssignableFrom(controllerType))
                    throw new ArgumentException(string.Format(
                        "Type requested is not a controller: {0}", controllerType.Name),
                        "controllerType");

                controller = ObjectFactory.GetInstance(controllerType) as IController;
            }

            return controller;
        }
    }
}
