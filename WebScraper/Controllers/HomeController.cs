using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebScraper.Models;
using WebScraperEngine;

namespace WebScraper.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var engineHelper = new EngineHelper();
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
                var profile = await engineHelper.CreateProfile(itemUrl, 
                    "$.champion.name", 
                    "$.champion.title", 
                    "$.champion.biography.short", 
                    null, null, null, null, null);
                profiles.Add(profile);
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

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
