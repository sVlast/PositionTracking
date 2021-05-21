using System;
using AngleSharp;
using System.Threading.Tasks;
using AngleSharp.Dom;
using System.Linq;
using System.Net.Http;
using AngleSharp.Io;
using System.IO;

namespace PositionTracking.Test
{
    class Program
    {
        static void Main(string[] args)
        {
        //var source = @"
        //<!DOCTYPE html>
        //<html lang=en>
        //  <meta charset=utf-8>
        //  <meta name=viewport content=""initial-scale=1, minimum-scale=1, width=device-width"">
        //  <title>Error 404 (Not Found)!!1</title>
        //  <style>
        //    *{margin:0;padding:0}html,code{font:15px/22px arial,sans-serif}html{background:#fff;color:#222;padding:15px}body{margin:7% auto 0;max-width:390px;min-height:180px;padding:30px 0 15px}* > body{background:url(//www.google.com/images/errors/robot.png) 100% 5px no-repeat;padding-right:205px}p{margin:11px 0 22px;overflow:hidden}ins{color:#777;text-decoration:none}a img{border:0}@media screen and (max-width:772px){body{background:none;margin-top:0;max-width:none;padding-right:0}}#logo{background:url(//www.google.com/images/errors/logo_sm_2.png) no-repeat}@media only screen and (min-resolution:192dpi){#logo{background:url(//www.google.com/images/errors/logo_sm_2_hr.png) no-repeat 0% 0%/100% 100%;-moz-border-image:url(//www.google.com/images/errors/logo_sm_2_hr.png) 0}}@media only screen and (-webkit-min-device-pixel-ratio:2){#logo{background:url(//www.google.com/images/errors/logo_sm_2_hr.png) no-repeat;-webkit-background-size:100% 100%}}#logo{display:inline-block;height:55px;width:150px}
        //  </style>
        //  <a href=//www.google.com/><span id=logo aria-label=Google></span></a>
        //  <p><b>404.</b> <ins>That’s an error.</ins>
        //  <p>The requested URL <code>/error</code> was not found on this server.  <ins>That’s all we know.</ins>";

            //Use the default configuration for AngleSharp

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
                //client.GetAsync(testUri);

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
                    var elem = document.QuerySelectorAll("div.g a");

                    Console.WriteLine("#search found");
                    
                    Console.WriteLine("");

                    var i = 0;

                    foreach (var item in elem)
                    {
                        i++;
                        Console.WriteLine(i+":");
                        Console.WriteLine("Text:"+item.Text());
                        Console.WriteLine("href:"+item.GetAttribute("href"));
                    }
                    Console.ReadLine();
                }
            }

            

            //Serialize it back to the console
            //Console.WriteLine(document.DocumentElement.OuterHtml);

        }
    }
}
