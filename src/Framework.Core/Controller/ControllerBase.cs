using Framework.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Framework.Core.Controller
{
    [ServiceFilter(typeof(CorrelationIdAttribute))]
    [ServiceFilter(typeof(ExceptionHandlerAttribute))]
    public class ControllerBase : Microsoft.AspNetCore.Mvc.Controller
    {
        public string CorrelationId { get; set; }

        public ControllerBase()
        {
        }
    }
}
