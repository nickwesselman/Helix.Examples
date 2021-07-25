using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.LayoutService.Client.Response.Model;

namespace BasicCompany.Project.BasicCompany
{
    public class MyPlaceholderJsonConverter : Sitecore.LayoutService.Client.Newtonsoft.Converters.PlaceholderJsonConverter
    {
        public override Placeholder ReadJson(JsonReader reader, Type objectType, Placeholder existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var collection = existingValue ?? new Placeholder();
            var jArray = JArray.Load(reader);

            foreach (var entry in jArray)
            {
                var componentName = entry["componentName"]?.ToString();
                var elementName = entry["name"]?.ToString();

                // closer to what JavaScript SDKs do
                // https://github.com/Sitecore/jss/blob/9114d2a5ae6cbba90a4dc0ce2486e1d20a68fa4d/packages/sitecore-jss-react/src/components/PlaceholderCommon.tsx#L144-L149
                if (string.IsNullOrEmpty(componentName) && !string.IsNullOrEmpty(elementName))
                {
                    collection.Add(serializer.Deserialize<EditableChrome>(entry.CreateReader()));
                }
                else if (!string.IsNullOrEmpty(componentName))
                {
                    collection.Add(serializer.Deserialize<Component>(entry.CreateReader()));
                }
            }

            return collection;
        }
    }
}
