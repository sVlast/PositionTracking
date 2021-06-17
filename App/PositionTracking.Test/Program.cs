using System;
using AngleSharp;
using System.Threading.Tasks;
using AngleSharp.Dom;
using System.Linq;
using System.Net.Http;
using AngleSharp.Io;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.Web;

namespace PositionTracking.Test
{

    class SearchContext
    {

        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
        //public string[] Pages { get; }
        public string NextPage { get; set; }
        public string Keyword { get; set; }
        public string Language { get; set; }
        public string Location { get; set; }
        public string Path { get; set; }

        public SearchContext(string keyword, string language, string location, string path)
        {
            MaxPage = 10;
            CurrentPage = 0;
            NextPage = "";
            //Pages = new string[MaxPage];
            Keyword = keyword;
            Language = language;
            Location = location;
            Path = path;

        }
    }

    class Program
    {

        private static readonly Random rnd = new Random();

        static IEnumerable<string> ParseSearchPage(IDocument document)
        {

            //document.querySelectorAll("div#rso div.g")
            //find first <a> from div.g with angle sharp
            return document.QuerySelectorAll("div#rso div.g")
                .Select(d => d.GetElementsByTagName("a").First())
                .Select(e => e.GetAttribute("href"));
        }

        static void SetNextPage(IDocument document,SearchContext context)
        {
            if(document == null)
            {
                context.NextPage = $"https://www.google.com/search?q={HttpUtility.UrlEncode(context.Keyword)}&cr=country{context.Location}&lr=lang_{context.Language}&pws=0";
            }
            else
            {
                context.NextPage = "https://www.google.com" + document.QuerySelector("a#pnnext").GetAttribute("href");
            }
        }
        static Stream GetSearchPage(SearchContext context)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");
                
                

                HttpResponseMessage httpResponse = client.GetAsync(context.NextPage).Result;
                var responseStream = httpResponse.Content.ReadAsStreamAsync().Result;



                using (var fileStream = File.Create("D:\\_Repos\\Testgrounds\\PositionTracking\\testpage" + context.CurrentPage + ".html"))
                {
                    responseStream.Seek(0, SeekOrigin.Begin);
                    responseStream.CopyTo(fileStream);
                    responseStream.Seek(0, SeekOrigin.Begin);
                }

                return responseStream;
            }

        }
        static int GetRating(string keyword, string language, string location, string path)
        {

            var searchContext = new SearchContext(keyword, language, location, path);
            var config = Configuration.Default;
            int rating = 0;

            SetNextPage(null, searchContext);

            while (searchContext.CurrentPage < searchContext.MaxPage)
            {
                
                var page = GetSearchPage(searchContext);

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

                        if (uri.Host.Contains(path, StringComparison.OrdinalIgnoreCase))
                            return rating;

                    }
                    if (rating == 0)
                    {
                        
                    };
                    searchContext.CurrentPage++;
                    Debug.WriteLine(searchContext.CurrentPage);
                    SetNextPage(document, searchContext);
                }
                
                Thread.Sleep(rnd.Next(3000, 7000));
            }

            return 0;
        }


        static void Main(string[] args)
        {


            var rating = GetRating("Klime", "HR", "HR", "www.aaaa.hr");

            Debug.WriteLine("Rating : " + rating);


            //using (var client = new HttpClient()) {

            //    //anglesharp configuration
            //    //var config = Configuration.Default;


            //    //var testUri = new Uri("https://www.google.com/");
            //    //var testString = "https://www.google.com/";
            //    //var testString2 = "https://www.google.com/search?q=mobilna+klima&client=firefox-b-d&sxsrf=ALeKk01fcsE1S-hayuHbMGF4Yy0Ej-bebg%3A1620910417371&ei=USGdYM_uFdOJ9u8P5-efkAs&oq=mobilna+klima&gs_lcp=Cgdnd3Mtd2l6EAMyAggAMgIIADICCAAyAggAMgIIADICCAAyBQgAEMsBMgUIABDLATIFCAAQywEyAggAOgcIABBHELADOgcIABCwAxBDOgQIIxAnOgQIABBDOggIABDHARCvAToICAAQsQMQgwE6CwgAELEDEMcBEK8BOgoIABDHARCjAhBDOgUIABCxAzoECAAQCjoCCC46BwgAELEDEAo6CggAEMcBEK8BEAo6CggAELEDEIMBEAo6BwgAEAoQywE6BQgAEMkDOgUIABCSA1DChRBY3KUQYKSoEGgGcAJ4AIABbogBiw6SAQQxNC41mAEAoAEBqgEHZ3dzLXdpesgBCsABAQ&sclient=gws-wiz&ved=0ahUKEwiP247t2cbwAhXThP0HHefzB7IQ4dUDCA0&uact=5";

            //    //Check UserAgent
            //    //client.DefaultRequestHeaders.UserAgent
            //    //client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");
            //    //HttpResponseMessage httpResponse = client.GetAsync(testString2).Result;


            //    using (var context = BrowsingContext.New(config))
            //    {


            //        //var responseStream = httpResponse.Content.ReadAsStreamAsync().Result;

            //        //using (var fileStream = File.Create("D:\\_Repos\\Testgrounds\\test"+".html")) {
            //        //    responseStream.Seek(0, SeekOrigin.Begin);
            //        //    responseStream.CopyTo(fileStream);
            //        //    responseStream.Seek(0, SeekOrigin.Begin);
            //        //}

            //        //var response = new DefaultResponse() {
            //        //    //
            //        //    Content = responseStream,
            //        //    Address = new Url(testString)
            //        //};

            //        //kako stream zapisat na hard disk i resetat stream-ov position nakon zapisivanja
            //        //newfile stream, copy to drugi stream
            //        //response.content.position RESET
            //        //ABBOT
            //        //var document = context.OpenAsync(response).Result;

            //        //Debug.WriteLine(document.DocumentElement.OuterHtml);

            //        //var elemlist = document.All.Where(m => m.Id == "search");
            //        //var elem = document.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("g"));
            //        //var elem = ParseSearchPage(document);

            //        //Debug.WriteLine("#search found");

            //        //Debug.WriteLine("");

            //        //var i = 0;

            //        //foreach (var item in elem)
            //        //{
            //        //    i++;
            //        //    Debug.WriteLine(i+":");
            //        //    Debug.WriteLine("Text:"+item);
            //        //    Debug.WriteLine("href:"+item);
            //        //}
            //        //Debug.ReadLine();
            //    }
            //}



            //Serialize it back to the Debug
            //Debug.WriteLine(document.DocumentElement.OuterHtml);

        }
    }
}
