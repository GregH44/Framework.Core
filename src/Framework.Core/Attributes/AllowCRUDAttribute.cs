using Framework.Core.Constants;
using Framework.Core.Controller;
using Framework.Core.Resolvers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace Framework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class AllowCRUDAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelName = context.ActionArguments.First(arg => arg.Key == "model").Value.ToString();
            var crudOperations = CrudResolver.GetCrudOperationsFromModelType(modelName);

            if (crudOperations != null)
            {
                switch (context.RouteData.Values.First(route => route.Key == "action").Value.ToString())
                {
                    case nameof(GenericController.Add):
                        if (!crudOperations[GlobalConstants.CrudOperations.Create])
                            context.Result = new UnauthorizedResult();
                        break;
                    case nameof(GenericController.Delete):
                        if (!crudOperations[GlobalConstants.CrudOperations.Delete])
                            context.Result = new UnauthorizedResult();
                        break;
                    case nameof(GenericController.Get):
                        if (!crudOperations[GlobalConstants.CrudOperations.Receive])
                            context.Result = new UnauthorizedResult();
                        break;
                    case nameof(GenericController.Update):
                        if (!crudOperations[GlobalConstants.CrudOperations.Update])
                            context.Result = new UnauthorizedResult();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
