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
        private const byte CreateIndex = 0;
        private const byte ReceiveIndex = 1;
        private const byte UpdateIndex = 2;
        private const byte DeleteIndex = 3;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelName = context.ActionArguments.Single(arg => arg.Key == "model").Value.ToString();
            var crudOperations = CrudResolver.GetCrudOperationsFromModelType(modelName);

            if (crudOperations != null)
            {
                switch (context.RouteData.Values.Single(route => route.Key == "action").Value.ToString())
                {
                    case nameof(GenericController.Add):
                        if (!crudOperations[CreateIndex])
                            context.Result = new UnauthorizedResult();
                        break;
                    case nameof(GenericController.Delete):
                        if (!crudOperations[DeleteIndex])
                            context.Result = new UnauthorizedResult();
                        break;
                    case nameof(GenericController.Get):
                        if (!crudOperations[ReceiveIndex])
                            context.Result = new UnauthorizedResult();
                        break;
                    case nameof(GenericController.Update):
                        if (!crudOperations[UpdateIndex])
                            context.Result = new UnauthorizedResult();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
