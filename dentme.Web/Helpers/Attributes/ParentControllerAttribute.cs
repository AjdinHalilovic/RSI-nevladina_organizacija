using System;

namespace nevladinaOrg.Web.Helpers.Attributes
{
    public class ParentControllerAttribute : Attribute
    {
        public string ControllerName { get; }

        public ParentControllerAttribute(string controllerName)
        {
            ControllerName = controllerName.RemoveControllerNameSuffix();
        }
    }
}