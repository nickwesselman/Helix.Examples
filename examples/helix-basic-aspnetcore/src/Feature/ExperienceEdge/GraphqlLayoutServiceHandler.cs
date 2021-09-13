using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.LayoutService.Client;
using Sitecore.LayoutService.Client.Exceptions;
using Sitecore.LayoutService.Client.Request;
using Sitecore.LayoutService.Client.Response;

namespace BasicCompany.Feature.ExperienceEdge
{
    public class GraphqlLayoutServiceHandler : Sitecore.LayoutService.Client.ILayoutRequestHandler
    {
        private readonly ISitecoreLayoutSerializer _serializer;
        private readonly ILogger<GraphqlLayoutServiceHandler> _logger;

        public GraphqlLayoutServiceHandler(
            ISitecoreLayoutSerializer serializer,
            ILogger<GraphqlLayoutServiceHandler> logger)
        {
            _serializer = serializer;
            _logger = logger;
        }

        public async Task<SitecoreLayoutResponse> Request(SitecoreLayoutRequest request, string handlerName)
        {
            var errors = new List<SitecoreLayoutServiceClientException>();
            SitecoreLayoutResponseContent? content = null;

            var graphqlClient = new GraphQLHttpClient("https://edge-beta.sitecorecloud.io/api/graphql/v1", new NewtonsoftJsonSerializer());
            graphqlClient.HttpClient.DefaultRequestHeaders.Add("sc_apikey", "TODO: config");
            var layoutRequest = new GraphQLRequest
            {
                Query = @"
                query LayoutQuery($path: String!, $language: String!, $site: String!) {
                    layout(routePath: $path, language: $language, site: $site) {
                        item {
                            rendered
                        }
                    }
                }",
                OperationName = "LayoutQuery",
                Variables = new
                {
                    path = request.Path(),
                    language = request.Language(),
                    site = request.SiteName()
                }
            };
            var response = await graphqlClient.SendQueryAsync<LayoutQueryResponse>(layoutRequest);
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"Layout Service GraphQL Response : {response.Data.Layout}");
            }

            var json = response.Data.Layout?["item"]?["rendered"]?.ToString();
            if (json == null)
            {
                errors.Add(new ItemNotFoundSitecoreLayoutServiceClientException());
            }
            else
            {
                content = _serializer.Deserialize(json);
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    var formattedDeserializeObject = JsonConvert.DeserializeObject(json);
                    _logger.LogDebug($"Layout Service Response JSON : {formattedDeserializeObject}");
                }
            }
            
            if (response.Errors != null)
            {
                errors.AddRange(
                    response.Errors.Select(e => new SitecoreLayoutServiceClientException(new LayoutServiceGraphqlException(e)))
                );
            }

            return new SitecoreLayoutResponse(request, errors)
            {
                Content = content,
                Metadata = new Dictionary<string, string>().ToLookup(k => k.Key, v => v.Value)
            };
        }

        private class LayoutQueryResponse
        {
            public JObject Layout { get; set; }
        }

        private class LayoutResponse
        {
            public ItemResponse Item { get; set; }
        }

        private class ItemResponse
        {
            public JObject Rendered { get; set; }
        }
    }
}
