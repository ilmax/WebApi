using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.OData.Routing
{
    public class ODataRouteContext : RouteContext
    {
        public ODataRouteContext(HttpContext httpContext) : base(httpContext)
        {
        }

        public ODataRouteContext(RouteContext other)
            :this(other.HttpContext)
        {
            Handler = other.Handler;
            RouteData = new RouteData(other.RouteData);
        }

        public ODataPath Path { get; set; }
    }
}
