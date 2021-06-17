using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using AngleSharp;
using AngleSharp.Io;
using AngleSharp.Dom;

namespace PositionTracking.Engine
{
    internal sealed class GoogleResolver
    {
        private const int _maxPageNum = 10;

        private static readonly Random _random = new Random();

        private readonly string _keyword;
        private readonly string _language;
        private readonly string _location;
        private readonly string _path;

        private string _nextPage;


        public GoogleResolver(string keyword, string language, string location, string path)
        {
            _keyword = keyword;
            _language = language;
            _location = location;
            _path = path;

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
                _nextPage = $"https://www.google.com/search?q={HttpUtility.UrlEncode(_keyword)}&cr=country{_location}&lr=lang_{_language}&pws=0";
            }
            else
            {
                _nextPage = "https://www.google.com" + document.QuerySelector("a#pnnext").GetAttribute("href");
            }
        }

        private Stream GetSearchPage()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");

                HttpResponseMessage httpResponse = client.GetAsync(_nextPage).Result;
                var responseStream = httpResponse.Content.ReadAsStreamAsync().Result;

                return responseStream;
            }

        }

        public int GetRank()
        {
            //var searchContext = new SearchContext(keyword, language, location, path);
            var config = Configuration.Default;
            int rating = 0;
            var pageNum = 0;

            SetNextPage(null);

            while (pageNum < _maxPageNum)
            {

                var page = GetSearchPage();

                using (var browsingContext = BrowsingContext.New(config))
                {

                    var response = new DefaultResponse()
                    {
                        Content = page,
                        Address = new Url(""),
                    };
                    var document = browsingContext.OpenAsync(response).Result;

                    var elems = ParseSearchPage(document);

                    foreach (var item in elems)
                    {
                        rating++;
                        Debug.WriteLine(rating + ":");
                        Debug.WriteLine("Text:" + item);
                        var uri = new Uri(item);
                        Debug.WriteLine("uri: " + uri);
                        Debug.WriteLine("uri.host: " + uri.Host);

                        if (uri.Host.IndexOf(_path, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return rating;
                        }
                    }
                    if (rating == 0)
                    {

                    };
                    pageNum++;
                    Debug.WriteLine(pageNum);
                    SetNextPage(document);
                }

                Thread.Sleep(_random.Next(3000, 7000));
            }

            return 0;
        }
    }
}
