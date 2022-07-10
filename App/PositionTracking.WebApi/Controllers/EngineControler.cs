﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PositionTracking.Data;
using PositionTracking.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace PositionTracking.WebApi.Controllers
{
    [AllowCrossSite]

    [ApiController]
    [Route("[controller]")]
    public class EngineController : ControllerBase
    {

        private readonly ILogger<EngineController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private static readonly string Screenshot_api_key = "6A8H5YG-MXZ4RHM-PW3X3KG-NB1KVTS";

        public EngineController(ILogger<EngineController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        [HttpGet("getinfo")]
        public ActionResult GetInfo()
        {
            return new OkResult();
        }

        [HttpGet("getrank/{id}")]
        public async Task<ActionResult<dynamic>> GetRank(Guid id)
        {
            const SearchEngineType searchEngine = SearchEngineType.GoogleWeb;


            var keyword = await _dbContext.Keywords
                .Include(k => k.Project)
                .FirstAsync(k => k.KeywordId == id);
                  

            //var result = 96;

            var result = await Resolver.GetRankAsync(
                keyword.Value,
                keyword.Language,
                keyword.Location,
                keyword.Project.Paths,
                searchEngine,
                _logger);


            keyword.Ratings = new List<KeywordRating>()
            {
                new KeywordRating(result, searchEngine)
            };

            await _dbContext.SaveChangesAsync();

            return new ActionResult<dynamic>(new
            {
                KeywordId = keyword.KeywordId,
                Rating = result
            });

        }

        [HttpGet("getscreenshot/{projectId}")]
        public async Task<ActionResult<string>> GetWebsiteScreenshot(Guid projectId)
        {
            var project = _dbContext.Projects.Where(p => p.ProjectId == projectId).First();

            if(project.webisteScreenshotUrl != null)
            {
                return project.webisteScreenshotUrl;
            }


            var url = "https://shot.screenshotapi.net";
            var queryParams = new Dictionary<string, string>()
            {
                {"token",Screenshot_api_key },
                {"file_type","png" },
                {"url",$"https://{project.Paths}" }
            };
            
            var parameters = QueryHelpers.AddQueryString("/screenshot", queryParams);
            string data;
            ScreenshotAPIData responseJSON;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                Console.WriteLine($"QUERY PARAMS:  {parameters}");
                HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    data = await response.Content.ReadAsStringAsync();
                    responseJSON = JsonConvert.DeserializeObject<ScreenshotAPIData>(data);

                    project.webisteScreenshotUrl = responseJSON.screenshotUrl;
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                }
                else
                {
                    data = "no response";
                    return "no response";
                }
            }

            return responseJSON.screenshotUrl;
        }
        

    }

    [Serializable]
    public class ScreenshotAPIData
    {
        [JsonProperty("screenshot")]
        public string screenshotUrl { get; set; }

        [JsonProperty("url")]
        public string originUrl { get; set; }

        [JsonProperty("created_at")]
        public string createdAt { get; set; }

        [JsonProperty("is_fresh")]
        public string isFresh { get; set; }

        [JsonProperty("token")]
        public string token { get; set; }

        [JsonProperty("file_type")]
        public string fileType { get; set; }

        [JsonProperty("ttl")]
        public string ttl { get; set; }
    }

}
