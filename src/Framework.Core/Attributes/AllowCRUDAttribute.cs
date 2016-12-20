using Framework.Core.Controller;
using Framework.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class AllowCRUDAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelName = context.ActionArguments.Single(arg => arg.Key == "model").Value.ToString();
            var crudOperationAllowed = ServiceResolver.GetModelType(modelName).GetTypeInfo().GetCustomAttribute<CrudOperationAttribute>();

            if (crudOperationAllowed != null && !string.IsNullOrEmpty(crudOperationAllowed.crudOperationsAllowed))
            {
                switch (context.RouteData.Values.Single(route => route.Key == "action").Value.ToString())
                {
                    case nameof(GenericController.Add):
                        if (!crudOperationAllowed.crudOperationsAllowed.Contains("C"))
                            context.Result = new UnauthorizedResult();
                        break;
                    case nameof(GenericController.Delete):
                        if (!crudOperationAllowed.crudOperationsAllowed.Contains("D"))
                            context.Result = new UnauthorizedResult();
                        break;
                    case nameof(GenericController.Get):
                    case nameof(GenericController.GetAll):
                        if (!crudOperationAllowed.crudOperationsAllowed.Contains("R"))
                            context.Result = new UnauthorizedResult();
                        break;
                    case nameof(GenericController.Update):
                        if (!crudOperationAllowed.crudOperationsAllowed.Contains("U"))
                            context.Result = new UnauthorizedResult();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
