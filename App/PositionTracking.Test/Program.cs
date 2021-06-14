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
        public string[] Pages { get; }
        public string Keyword { get; set; }
        public string Language { get; set; }
        public string Location { get; set; }
        public string Path { get; set; }

        public SearchContext(string keyword,string language,string location,string path ) {
            MaxPage = 10;
            Pages = new string[MaxPage];
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
            return document.QuerySelectorAll("div.g a:not(a[class])").Select(e => e.GetAttribute("href")) ;
            
        }
        static Stream getSearchPage(SearchContext context)
        {
            string url;
            
            if(context.CurrentPage == 0)
            {
                url = "http://www.google.com/search?q="+context.Keyword+"&cr=country"+context.Location+"&hl=lang_" ;
            }
            else
            {

            }

            using (var client = new HttpClient())
            { 
                
            }
            return 
        }
        static int GetRating(string keyword,string language, string location,string path) {
            var context = new SearchContext(keyword,language,location,path);
            var page = getSearchPage(context);
        }


        static void Main(string[] args)
        {
            //IBrowsingContext context = null;
            //try
            //{
            //    //Create a new context for evaluating webpages with the given config
            //    context = BrowsingContext.New(config);


            //    //Just get the DOM representation
            //    var document = context.OpenAsync(req => req.Content(source)).Result;

            //    var elemlist = document.All.Where(m => m.LocalName == "ins");

            //    foreach (var item in elemlist)
            //    {
            //        Console.WriteLine(item.Text());
            //    }
            //}
            //finally
            //{

            //    //var c = context as IDisposable;
            //    //if (c != null)
            //    //    c.Dispose();

            //    (context as IDisposable)?.Dispose();

            //}


            using (var client = new HttpClient()) {

                //anglesharp configuration
                var config = Configuration.Default;


                //var testUri = new Uri("https://www.google.com/");
                var testString = "https://www.google.com/";
                var testString2 = "https://www.google.com/search?q=mobilna+klima&client=firefox-b-d&sxsrf=ALeKk01fcsE1S-hayuHbMGF4Yy0Ej-bebg%3A1620910417371&ei=USGdYM_uFdOJ9u8P5-efkAs&oq=mobilna+klima&gs_lcp=Cgdnd3Mtd2l6EAMyAggAMgIIADICCAAyAggAMgIIADICCAAyBQgAEMsBMgUIABDLATIFCAAQywEyAggAOgcIABBHELADOgcIABCwAxBDOgQIIxAnOgQIABBDOggIABDHARCvAToICAAQsQMQgwE6CwgAELEDEMcBEK8BOgoIABDHARCjAhBDOgUIABCxAzoECAAQCjoCCC46BwgAELEDEAo6CggAEMcBEK8BEAo6CggAELEDEIMBEAo6BwgAEAoQywE6BQgAEMkDOgUIABCSA1DChRBY3KUQYKSoEGgGcAJ4AIABbogBiw6SAQQxNC41mAEAoAEBqgEHZ3dzLXdpesgBCsABAQ&sclient=gws-wiz&ved=0ahUKEwiP247t2cbwAhXThP0HHefzB7IQ4dUDCA0&uact=5";

                //Check UserAgent
                //client.DefaultRequestHeaders.UserAgent
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");
                HttpResponseMessage httpResponse = client.GetAsync(testString2).Result;


                using (var context = BrowsingContext.New(config))
                {
                    //Just get the DOM representation
                    //var document = context.OpenAsync(req => req.Content(source)).Result;

                    //var http = new Url("https://www.google.com/search?q=Best+races&sxsrf=ALeKk03S_R7mYO5z5kGjBVMzobdsdNinNg%3A1620051027482&source=hp&ei=UwSQYLGlG46GjLsPwZibiAQ&iflsig=AINFCbYAAAAAYJASY6l679y_90zG1A0DBXERexnoZGN9&oq=Best+races&gs_lcp=Cgdnd3Mtd2l6EAMyBQgAEMsBMgUIABDLATIFCAAQywEyBQgAEMsBMgUIABDLATIFCAAQywEyBQgAEMsBMgUIABDLATIFCAAQywEyBQgAEMsBOgcIIxDqAhAnOgkIIxDqAhAnEBM6BAgjECc6BQgAELEDOggIABCxAxCDAToICC4QsQMQgwE6CwgAELEDEMcBEKMCOgYIIxAnEBM6BQguELEDOgIIADoCCC46CAgAEMcBEK8BUIpCWIGMAmC9lwJoB3AAeACAAZ4BiAHkC5IBBDQuMTCYAQCgAQGqAQdnd3Mtd2l6sAEK&sclient=gws-wiz&ved=0ahUKEwjxmomw2K3wAhUOA2MBHUHMBkEQ4dUDCAY&uact=5");
                    //var t = context.OpenAsync(http);
                    //t.Wait();

                    var responseStream = httpResponse.Content.ReadAsStreamAsync().Result;

                    using (var fileStream = File.Create("D:\\_Repos\\Testgrounds\\test"+".html")) {
                        responseStream.Seek(0, SeekOrigin.Begin);
                        responseStream.CopyTo(fileStream);
                        responseStream.Seek(0, SeekOrigin.Begin);
                    }

                    var response = new DefaultResponse() {
                        //
                        Content = responseStream,
                        Address = new Url(testString)
                    };

                    //kako stream zapisat na hard disk i resetat stream-ov position nakon zapisivanja
                    //newfile stream, copy to drugi stream
                    //response.content.position RESET
                    //ABBOT
                    var document = context.OpenAsync(response).Result;

                    //Console.WriteLine(document.DocumentElement.OuterHtml);

                    //var elemlist = document.All.Where(m => m.Id == "search");
                    //var elem = document.All.Where(m => m.LocalName == "div" && m.ClassList.Contains("g"));
                    var elem = ParseSearchPage(document);

                    Console.WriteLine("#search found");
                    
                    Console.WriteLine("");

                    var i = 0;

                    foreach (var item in elem)
                    {
                        i++;
                        Console.WriteLine(i+":");
                        Console.WriteLine("Text:"+item);
                        Console.WriteLine("href:"+item);
                    }
                    Console.ReadLine();
                }
            }

            

            //Serialize it back to the console
            //Console.WriteLine(document.DocumentElement.OuterHtml);

        }
    }
}
