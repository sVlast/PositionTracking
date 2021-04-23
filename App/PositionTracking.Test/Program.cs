using System;
using AngleSharp;
using System.Threading.Tasks;
using AngleSharp.Dom;
using System.Linq;

namespace PositionTracking.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = @"
<!DOCTYPE html>
<html lang=en>
  <meta charset=utf-8>
  <meta name=viewport content=""initial-scale=1, minimum-scale=1, width=device-width"">
  <title>Error 404 (Not Found)!!1</title>
  <style>
    *{margin:0;padding:0}html,code{font:15px/22px arial,sans-serif}html{background:#fff;color:#222;padding:15px}body{margin:7% auto 0;max-width:390px;min-height:180px;padding:30px 0 15px}* > body{background:url(//www.google.com/images/errors/robot.png) 100% 5px no-repeat;padding-right:205px}p{margin:11px 0 22px;overflow:hidden}ins{color:#777;text-decoration:none}a img{border:0}@media screen and (max-width:772px){body{background:none;margin-top:0;max-width:none;padding-right:0}}#logo{background:url(//www.google.com/images/errors/logo_sm_2.png) no-repeat}@media only screen and (min-resolution:192dpi){#logo{background:url(//www.google.com/images/errors/logo_sm_2_hr.png) no-repeat 0% 0%/100% 100%;-moz-border-image:url(//www.google.com/images/errors/logo_sm_2_hr.png) 0}}@media only screen and (-webkit-min-device-pixel-ratio:2){#logo{background:url(//www.google.com/images/errors/logo_sm_2_hr.png) no-repeat;-webkit-background-size:100% 100%}}#logo{display:inline-block;height:55px;width:150px}
  </style>
  <a href=//www.google.com/><span id=logo aria-label=Google></span></a>
  <p><b>404.</b> <ins>That’s an error.</ins>
  <p>The requested URL <code>/error</code> was not found on this server.  <ins>That’s all we know.</ins>";

            //Use the default configuration for AngleSharp
            var config = Configuration.Default;

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

            using (var context = BrowsingContext.New(config))
            {
                


                //Just get the DOM representation
                var document = context.OpenAsync(req => req.Content(source)).Result;



                var elemlist = document.All.Where(m => m.LocalName == "ins");

                foreach (var item in elemlist)
                {
                    Console.WriteLine(item.Text());
                }
            }

            //Serialize it back to the console
            //Console.WriteLine(document.DocumentElement.OuterHtml);

        }
    }
}
