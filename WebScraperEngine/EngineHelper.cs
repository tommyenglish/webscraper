using Manatee.Json;
using Manatee.Json.Path;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace WebScraperEngine
{
    public class EngineHelper
    {
        public async Task<IList<string>> GetItemUrls(IList<string> feedUrls, string xpath)
        {
            var result = new List<string>();
            var browser = new ScrapingBrowser
            {
                AllowAutoRedirect = true,
                AllowMetaRedirect = true
            };

            foreach (var feedUrl in feedUrls)
            {
                var uri = new Uri(feedUrl);
                var pageResult = await browser.NavigateToPageAsync(uri);
                var itemUrlNode = pageResult.Html.SelectSingleNode(xpath);
                string itemUrl = itemUrlNode.InnerText;
                result.Add(itemUrl);
            }

            return result;
        }

        public async Task<IList<string>> GetItemUrlsAjax(IList<string> feedUrls, string jsonPath, string interpolationUrl = null)
        {
            var itemUrlList = new List<string>();
            var browser = new ScrapingBrowser
            {
                AllowAutoRedirect = true,
                AllowMetaRedirect = true,

            };

            foreach (var feedUrl in feedUrls)
            {
                var uri = new Uri(feedUrl);
                var response = await browser.AjaxDownloadStringAsync(uri);
                var json = JsonValue.Parse(response);
                var path = JsonPath.Parse(jsonPath);
                var itemUrls = path.Evaluate(json);
                itemUrlList.AddRange(itemUrls.Select(i => string.Format(interpolationUrl ?? "{0}", i.String)));
            }

            return itemUrlList;
        }
    }
}
