using Framework.Core.Constants;
using Framework.Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace Framework.Core.Attributes
{
    public class CorrelationIdAttribute : ActionFilterAttribute, IActionFilter
    {
        private ILogger<CorrelationIdAttribute> logger = null;

        public CorrelationIdAttribute(ILogger<CorrelationIdAttribute> logger)
        {
            this.logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var correlationId = ObtenirCorrelationId(context.HttpContext);

            // Ajout du CorrelationId dans le contrôleur
            var controllerCorrelationIdInfo = context.Controller.GetType().GetProperty(GlobalConstants.CorrelationIdName);

            if (controllerCorrelationIdInfo != null)
            {
                controllerCorrelationIdInfo.SetValue(context.Controller, correlationId);
            }

            context.HttpContext.Items[GlobalConstants.CorrelationIdName] = correlationId;
        }

        private string ObtenirCorrelationId(HttpContext context)
        {
            string account = string.Empty;
            string correlationId = string.Empty;
            string message = "Création de l'identifiant de corrélation pour l'utilisateur {0} : {1}";

            if (context.User.Claims.Count() > 0)
            {
                account = context.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            }
            else
            {
                account = "Unidentified";
            }

            correlationId = GlobalHelpers.GenererCorrelationId(account);
            logger.LogInformation(string.Format(message, account, correlationId));

            return correlationId;
        }
    }
}
