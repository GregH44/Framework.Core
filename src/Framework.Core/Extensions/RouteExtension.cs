using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Framework.Core.Extensions
{
    public static class RouteExtension
    {
        /// <summary>
        /// Route template to use : {model}/{action}/{id?}
        /// </summary>
        public static void MapGenericRoute(this IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                    name: "Generic API",
                    template: "{model}/{action}/{id?}",
                    defaults: new { controller = "Generic" });
        }
    }
}
