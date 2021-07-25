using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sitecore.LayoutService.Client.Newtonsoft.Converters;

namespace BasicCompany.Project.BasicCompany
{
    public static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings WithExtensions(this JsonSerializerSettings settings)
        {
            var converters = settings.Converters;
            var placeholderConverter = converters.Single(x => x is PlaceholderJsonConverter);
            converters.Remove(placeholderConverter);
            converters.Add(new MyPlaceholderJsonConverter());
            return settings;
        }
    }
}
