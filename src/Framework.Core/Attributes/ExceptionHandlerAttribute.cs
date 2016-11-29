using Framework.Core.Constants;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Framework.Core.Attributes
{
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger logger;

        public ExceptionHandlerAttribute(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("ExceptionHandlerFilter");
        }

        public override void OnException(ExceptionContext context)
        {
            if (context != null)
            {
                StringBuilder message = new StringBuilder();
                string correlationId = "Inconnu";

                if (context.HttpContext != null
                    && context.HttpContext.Items != null && context.HttpContext.Items.Count() > 0
                    && context.HttpContext.Items.ContainsKey(GlobalConstants.CorrelationIdName))
                {
                    correlationId = context.HttpContext.Items[GlobalConstants.CorrelationIdName].ToString();
                }

                string urlDonneesContexte = ObtenirUrlEtDonneesContexte(context);

                if (context.Exception.InnerException == null)
                {
                    message.AppendLine($"Trace : {context.Exception.Message} ({GlobalConstants.CorrelationIdName} : {correlationId} || Données : {urlDonneesContexte})");
                    message.AppendLine($"  => {context.Exception.StackTrace}");
                }
                else
                {
                    // Le message correspondant aux exceptions du ServiceLayer, DataAccessLayer et GenericRepository contient le CorrelationId
                    message.AppendLine($"Trace : {context.Exception.Message} (({GlobalConstants.CorrelationIdName} : {correlationId} || Données : {urlDonneesContexte})");
                    message.AppendLine($"  => {context.Exception.StackTrace}");
                    message.AppendLine($"Inner Exception : {context.Exception.InnerException.Message} ({correlationId})");
                    message.AppendLine($"  => {context.Exception.InnerException.StackTrace}");
                }

                logger.LogCritical(message.ToString());
            }
            else
                logger.LogCritical("Le contexte de l'exception vaut null !!!");
        }

        private string ObtenirUrlEtDonneesContexte(ExceptionContext context)
        {
            var buildDatas = new StringBuilder("{");

            buildDatas.Append("Datas : ");

            if (context.RouteData.Values.Count > 0)
            {
                buildDatas.Append("{ ");

                foreach (var kvp in context.RouteData.Values)
                {
                    buildDatas.AppendFormat("{0}={1} ", kvp.Key, kvp.Value);
                }

                buildDatas.Append("}");
            }
            else
                buildDatas.Append("Aucune");

            buildDatas.Append("}");

            return buildDatas.ToString();
        }
    }
}
