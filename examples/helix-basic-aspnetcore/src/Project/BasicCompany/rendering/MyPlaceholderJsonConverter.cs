using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sitecore.LayoutService.Client.Response.Model;

namespace BasicCompany.Project.BasicCompany
{
    public class MyPlaceholderJsonConverter : Sitecore.LayoutService.Client.Newtonsoft.Converters.PlaceholderJsonConverter
    {
        public override Placeholder ReadJson(JsonReader reader, Type objectType, Placeholder existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Console.WriteLine("Got me!");
            return base.ReadJson(reader, objectType, existingValue, hasExistingValue, serializer);
        }
    }
}
