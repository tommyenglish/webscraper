using Manatee.Json;
using Manatee.Json.Path;
using System.Linq;

namespace WebScraperEngine
{
    internal class JsonHelper
    {
        public string GetStringValue(JsonValue jsonValue, string jsonPath)
        {
            if (string.IsNullOrEmpty(jsonPath))
            {
                return string.Empty;
            }

            var path = JsonPath.Parse(jsonPath);
            var result = path.Evaluate(jsonValue).FirstOrDefault();

            return result?.String ?? string.Empty;
        }
    }
}
