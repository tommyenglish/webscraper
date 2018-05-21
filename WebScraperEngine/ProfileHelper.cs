using Manatee.Json;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebScraperEngine.Models;

namespace WebScraperEngine
{
    public class ProfileHelper
    {
        public async Task<Profile> CreateProfileAjax(string itemUrl, string firstNamePath, string lastNamePath, string bioPath, string imageUrlPath,
            string emailPath, string groupsPath, string skillsPath, string pathwaysPath)
        {
            var jsonHelper = new JsonHelper();
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
                FirstName = jsonHelper.GetStringValue(jsonValue, firstNamePath),
                LastName = jsonHelper.GetStringValue(jsonValue, lastNamePath),
                Bio = jsonHelper.GetStringValue(jsonValue, bioPath),
                ImageUrl = jsonHelper.GetStringValue(jsonValue, imageUrlPath),
                Email = jsonHelper.GetStringValue(jsonValue, emailPath)
            };

            return profile;
        }

        public async Task CreateProfileAjaxAdd(List<Profile> profiles, string itemUrl, string firstNamePath, string lastNamePath, string bioPath, string imageUrlPath,
            string emailPath, string groupsPath, string skillsPath, string pathwaysPath)
        {
            var jsonHelper = new JsonHelper();
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
                FirstName = jsonHelper.GetStringValue(jsonValue, firstNamePath),
                LastName = jsonHelper.GetStringValue(jsonValue, lastNamePath),
                Bio = jsonHelper.GetStringValue(jsonValue, bioPath),
                ImageUrl = jsonHelper.GetStringValue(jsonValue, imageUrlPath),
                Email = jsonHelper.GetStringValue(jsonValue, emailPath)
            };

            profiles.Add(profile);
        }
    }
}
