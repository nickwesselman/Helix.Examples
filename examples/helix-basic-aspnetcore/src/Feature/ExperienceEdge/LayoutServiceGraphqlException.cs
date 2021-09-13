using System;
using System.Collections.Generic;
using System.Text;
using GraphQL;
using Sitecore.LayoutService.Client.Exceptions;

namespace BasicCompany.Feature.ExperienceEdge
{
    public class LayoutServiceGraphqlException : SitecoreLayoutServiceClientException
    {
        public GraphQLError GraphQLError { get; private set; }

        public LayoutServiceGraphqlException(GraphQLError error) : base(error.Message)
        {
            GraphQLError = error;
        }
    }
}
