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
            var result = new List<string>();
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
                result.AddRange(itemUrls.Select(i => string.Format(interpolationUrl ?? "{0}", i.String)));
            }

            return result;
        }

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

        public async Task<Profile> CreateProfile(string itemUrl, string firstNamePath, string lastNamePath, string bioPath, string imageUrlPath, 
            string emailPath, string groupsPath, string skillsPath, string pathwaysPath)
        {
            var browser = new ScrapingBrowser
            {
                AllowAutoRedirect = true,
                AllowMetaRedirect = true,

            };
            var uri = new Uri(itemUrl);
            var response = await browser.AjaxDownloadStringAsync(uri);
            var jsonValue = JsonValue.Parse(response);

            var profile = new Profile
            {
                FirstName = GetStringValue(jsonValue, firstNamePath),
                LastName = GetStringValue(jsonValue, lastNamePath),
                Bio = GetStringValue(jsonValue, bioPath),
                ImageUrl = GetStringValue(jsonValue, imageUrlPath),
                Email = GetStringValue(jsonValue, emailPath)
            };

            return profile;
        }
    }

    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Groups => Enumerable.Empty<string>();
        public IEnumerable<string> Skills => Enumerable.Empty<string>();
        public IEnumerable<string> Pathways => Enumerable.Empty<string>();
    }
}
