using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using PositionTracking.Data;

namespace PositionTracking.WebApi
{
    public class ScreenshotApiHandler
    {

        private static readonly string Screenshot_api_key = "6A8H5YG-MXZ4RHM-PW3X3KG-NB1KVTS";

        public ScreenshotApiHandler()
        {  
        }

        public async Task<string>  getScreenshot(Project project, ApplicationDbContext _dbContext)
        {
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
                    return "Error: response unsuccessful.";
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

