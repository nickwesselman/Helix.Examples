using System;
using System.Threading.Tasks;
using Sitecore.LayoutService.Client.Request;
using Sitecore.LayoutService.Client.Response;

namespace BasicCompany.Feature.ExperienceEdge
{
    public class GraphqlLayoutServiceHandler : Sitecore.LayoutService.Client.ILayoutRequestHandler
    {
        public Task<SitecoreLayoutResponse> Request(SitecoreLayoutRequest request, string handlerName)
        {
            throw new NotImplementedException();
        }
    }
}
