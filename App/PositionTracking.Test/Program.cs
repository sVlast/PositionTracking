using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Diagnostics;
using System.Web;
using AngleSharp;
using AngleSharp.Io;
using AngleSharp.Dom;
using PositionTracking.Engine;
using PositionTracking.Data;

namespace PositionTracking.Test
{


    class Program
    {

        static void Main(string[] args)
        {

            using (var db = new ApplicationDbContext())
            {
                Console.WriteLine(db.Users.Count());
            }





            //var rank = Resolver.GetRank("klime", Languages.lang_hr, Countries.HR,"www.klime.hr", ResolverType.GoogleWeb);

            //Console.WriteLine("Testing Rank : "+rank);
            //Console.ReadLine();
        }
    }
}
