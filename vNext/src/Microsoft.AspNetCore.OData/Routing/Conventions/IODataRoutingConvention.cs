using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Microsoft.AspNetCore.OData.Routing.Conventions
{
    public interface IODataRoutingConvention
    {
        ActionDescriptor SelectAction(RouteContext routeContext);
    }
}