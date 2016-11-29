using Framework.Core.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Framework.Core.Controller
{
    [ServiceFilter(typeof(CorrelationIdAttribute))]
    [ServiceFilter(typeof(ExceptionHandlerAttribute))]
    public class ControllerBase : Microsoft.AspNetCore.Mvc.Controller
    {
        public string CorrelationId { get; set; }
        protected ILogger logger = null;

        public ControllerBase(ILogger logger)
        {
            this.logger = logger;
        }
    }
}
