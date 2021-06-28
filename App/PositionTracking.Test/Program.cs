using Microsoft.EntityFrameworkCore;
using PositionTracking.Data;
using PositionTracking.Engine;
using System;
using System.Linq;

namespace PositionTracking.Test
{


    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Test:");
            using (var db = new ApplicationDbContext())
            {

                Resolver.UpdateRanks(db);
            }

            using (var db = new ApplicationDbContext())
            {
                foreach (var item in db.Keywords.Include(k=>k.Ratings))
            {
                Console.WriteLine(item.Value + " ranks: ");
                foreach(var rating in item.Ratings)
                {
                    Console.WriteLine(" -- " + rating.Rank);
                }
            }
            Console.ReadLine();
            }



            //var rank = Resolver.GetRank("klime", Languages.lang_hr, Countries.HR,"www.klime.hr", ResolverType.GoogleWeb);

            //Console.WriteLine("Testing Rank : "+rank);
            //Console.ReadLine();
        }
    }
}
