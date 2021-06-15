using System;
using AngleSharp;
using System.Threading.Tasks;
using AngleSharp.Dom;
using System.Linq;
using System.Net.Http;
using AngleSharp.Io;
using System.IO;
using System.Collections.Generic;

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

        static IEnumerable<string> ParseSearchPage(IDocument document)
        {

            //document.querySelectorAll("div#rso div.g")
            //find first <a> from div.g with angle sharp
            return document.QuerySelectorAll("div#rso div.g").Select(d => d.GetElementsByTagName("a").First()).Select(e => e.GetAttribute("href"));

        }
        static Stream GetSearchPage(SearchContext context)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");
                string url = "";

                if (context.CurrentPage == 0)
                {
                    //cr za lokaciju, lr za jezik dokumenta
                    url = "http://www.google.com/search?q=" + context.Keyword + "&cr=country" + context.Location + "&lr=lang_" + context.Language + "&pws=0";
                }
                else
                {
                    url = "http://www.google.com/search?q=" + context.Keyword + "&cr=country" + context.Location + "&lr=lang_" + context.Language + "&pws=0" + "&start=" + context.CurrentPage*10 ;
                }
                HttpResponseMessage httpResponse = client.GetAsync(url).Result;
                var responseStream = httpResponse.Content.ReadAsStreamAsync().Result;



                using (var fileStream = File.Create("D:\\_Repos\\Testgrounds\\page" + context.CurrentPage + ".html"))
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
            while (searchContext.CurrentPage < 1)
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
                        Console.WriteLine(rating + ":");
                        Console.WriteLine("Text:" + item);
                        var uri = new Uri(item);
                        Console.WriteLine("uri: " + uri);
                        Console.WriteLine("uri.host: " + uri.Host);

                        if (uri.Host.Contains(path, StringComparison.OrdinalIgnoreCase))
                            return rating;

                    }
                    if (rating == 0)
                    {
                        searchContext.CurrentPage++;
                        Console.WriteLine(searchContext.CurrentPage);
                    };
                }
            }
            
            return 0;
        }


        static void Main(string[] args)
        {


            var rating = GetRating("Klime", "HR", "HR", "www.elipso.hr");

            Console.WriteLine("Rating : " + rating);
            Console.ReadLine();


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

            //        //Console.WriteLine(document.DocumentElement.OuterHtml);

            //        //var elemlist = document.All.Where(m => m.Id == "search");
            //        //var elem = document.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("g"));
            //        //var elem = ParseSearchPage(document);

            //        //Console.WriteLine("#search found");

            //        //Console.WriteLine("");

            //        //var i = 0;

            //        //foreach (var item in elem)
            //        //{
            //        //    i++;
            //        //    Console.WriteLine(i+":");
            //        //    Console.WriteLine("Text:"+item);
            //        //    Console.WriteLine("href:"+item);
            //        //}
            //        //Console.ReadLine();
            //    }
            //}



            //Serialize it back to the console
            //Console.WriteLine(document.DocumentElement.OuterHtml);

        }
    }
}
