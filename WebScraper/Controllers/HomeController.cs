using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebScraper.Models;
using WebScraperEngine;
using WebScraperEngine.Models;

namespace WebScraper.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var engineHelper = new EngineHelper();
            var profileHelper = new ProfileHelper();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var feedUrls = new List<string>
            {
                @"https://universe-meeps.leagueoflegends.com/v1/en_us/champion-browse/index.json"
            };
            string jsonPath = @"$.champions[*].slug";
            string interpolationUrl = @"https://universe-meeps.leagueoflegends.com/v1/en_us/champions/{0}/index.json";
            var itemUrls = await engineHelper.GetItemUrlsAjax(feedUrls, jsonPath, interpolationUrl);
            var profiles = new List<Profile>();
            foreach (var itemUrl in itemUrls)
            {
                var profile = await profileHelper.CreateProfileAjax(itemUrl, 
                    "$.champion.name", 
                    "$.champion.title", 
                    "$.champion.biography.short", 
                    null, null, null, null, null);
                profiles.Add(profile);
            }
            stopWatch.Stop();
            var elapsed = stopWatch.Elapsed.TotalSeconds;
            return View();
        }

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "Your application description page.";

            var engineHelper = new EngineHelper();
            var profileHelper = new ProfileHelper();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var feedUrls = new List<string>
            {
                @"https://universe-meeps.leagueoflegends.com/v1/en_us/champion-browse/index.json"
            };
            string jsonPath = @"$.champions[*].slug";
            string interpolationUrl = @"https://universe-meeps.leagueoflegends.com/v1/en_us/champions/{0}/index.json";
            var itemUrls = await engineHelper.GetItemUrlsAjax(feedUrls, jsonPath, interpolationUrl);
            var profiles = new List<Profile>();
            var taskList = new List<Task>();
            foreach (var itemUrl in itemUrls)
            {
                taskList.Add(profileHelper.CreateProfileAjaxAdd(profiles, itemUrl,
                    "$.champion.name",
                    "$.champion.title",
                    "$.champion.biography.short",
                    null, null, null, null, null));
            }

            await Task.WhenAll(taskList);
            stopWatch.Stop();
            var elapsed = stopWatch.Elapsed.TotalSeconds;
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
