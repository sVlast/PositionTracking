using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Io;
using Microsoft.Extensions.Logging;
using PositionTracking.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PositionTracking.Engine
{
    internal sealed class GoogleResolver : IDisposable
    {
        private const int _maxPageNum = 10;

        private static DateTime _reqTimeStamp;
        private static readonly Random _random = new Random();
        private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,1);

        private readonly string _keyword;
        private readonly string _language;
        private readonly Countries _location;
        private readonly string _path;
        private readonly ILogger _logger;

        private string _nextPage;

        //Implement synchronizer to block incoming requests

        public GoogleResolver(string keyword, Languages language, Countries location, string path, ILogger logger)
        {
            _logger = logger ?? new DummyLogger();
            _keyword = keyword;
            _language = GetLang(language);
            _location = location;
            _path = path;
        }

        private static string GetLang(Languages lang)
        {
            switch (lang)
            {
                case Languages.en:
                    return "lang_en";
                case Languages.bs:
                case Languages.hr:
                    return "lang_hr";
                case Languages.sr:
                    return "lang_sr";
                case Languages.sl:
                    return "lang_sl";
                default:
                    throw new NotSupportedException();

            }
        }

        private static IEnumerable<string> ParseSearchPage(IDocument document)
        {
            return document.QuerySelectorAll("div#rso div.g")
                .Select(d => d.GetElementsByTagName("a").First())
                .Select(e => e.GetAttribute("href"));
        }

        private void SetNextPage(IDocument document)
        {
            if (document == null)
            {
                _nextPage = $"https://www.google.com/search?q={HttpUtility.UrlEncode(_keyword)}&cr=country{_location}&lr={_language}&pws=0";
            }
            else
            {
                var e = document.QuerySelector("a#pnnext");
                _nextPage = e == null ? null : "https://www.google.com" + e.GetAttribute("href");
            }
        }

        private async Task<Stream> GetSearchPageAsync()
        {
            //each call checks if there is a free slot else waits until semaphore.Release()
            await _semaphoreSlim.WaitAsync();

            try
            {
                // calulate time diference between bettween the current and the last call to method
                var timediff = TimeSpan.FromMilliseconds(_random.Next(3000, 7000)) - ( DateTime.UtcNow - _reqTimeStamp);
                if (timediff > TimeSpan.Zero) 
                {
                    _logger.LogDebug("Waiting " + timediff);
                    await Task.Delay(timediff);
                }
            }
            finally
            {
                _reqTimeStamp = DateTime.UtcNow;
                _semaphoreSlim.Release();
            }

            //synchronize this method
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");

                HttpResponseMessage httpResponse = await client.GetAsync(_nextPage);

                var responseStream = await httpResponse.Content.ReadAsStreamAsync();
                //
                return responseStream;
            }
        }

        public async Task<int> GetRankAsync()
        {
            //var searchContext = new SearchContext(keyword, language, location, path);
            var config = Configuration.Default;
            int rating = 0;
            var pageNum = 0;

            SetNextPage(null);

            while (pageNum < _maxPageNum && _nextPage != null)
            {
                _logger.LogDebug($"Fetching page ({ pageNum}) {_nextPage}");
                var page = await GetSearchPageAsync();

                using (var browsingContext = BrowsingContext.New(config))
                {

                    var response = new DefaultResponse()
                    {
                        Content = page,
                        Address = new Url(""),
                    };
                    _logger.LogDebug("Opening page " + _nextPage);
                    var document = await browsingContext.OpenAsync(response);

                    _logger.LogDebug("Parsing page " + _nextPage);
                    var elems = ParseSearchPage(document);

                    foreach (var item in elems)
                    {
                        rating++;
                        try
                        {
                            //error handling
                            if (item == "#")
                                throw new FormatException("Invalid link format (#).");

                            _logger.LogDebug("Checking uri " + item + " against " + _path);
                            var uri = new Uri(item);

                            //implement better string comparison
                            if (uri.Host.IndexOf(_path, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                return rating;
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogWarning(e.Message);
                        }

                    }
                    if (rating == 0)
                    {
                        //todo: implement error handling
                        
                        _logger.LogError("Google parser found no matching elements : ", _path, _keyword, _language,_location); ;
                    };
                    pageNum++;

                    SetNextPage(document);
                }

            }

            return 0;
        }

        public void Dispose()
        {
            _semaphoreSlim.Dispose();
        }
    }
}
