using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.LayoutService.Client;
using Sitecore.LayoutService.Client.Newtonsoft;
using Sitecore.LayoutService.Client.Newtonsoft.Extensions;
using Sitecore.LayoutService.Client.Response;

namespace BasicCompany.Project.BasicCompany
{
    public class MyLayoutServiceSerializer : ISitecoreLayoutSerializer
    {
        private static readonly Lazy<JsonSerializerSettings> _settings = new Lazy<JsonSerializerSettings>(CreateSerializerSettings);

        /// <inheritdoc/>
        public SitecoreLayoutResponseContent Deserialize(string data)
        {
            var deserializedObject = JsonConvert.DeserializeObject<SitecoreLayoutResponseContent>(data, _settings.Value);

            var jObject = JObject.Parse(data);
            deserializedObject.ContextRawData = jObject["sitecore"]?["context"]?.ToString()!;
            return deserializedObject;
        }

        /// <summary>
        /// Gets the default settings for the JSON serializer.
        /// </summary>
        /// <returns>An instance of <see cref="JsonSerializerSettings"/> with all serialization defaults for the Sitecore layout service.</returns>
        public static JsonSerializerSettings CreateSerializerSettings()
        {
            return new JsonSerializerSettings().SetDefaults().WithExtensions();
        }
    }
}
