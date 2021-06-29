using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PositionTracking.Data;

namespace PositionTracking.Engine
{
    public static class Resolver
    {
        
        public static int GetRank(string keyword, Languages language, Countries location, string path,SearchEngineType searchEngine)
        {
            switch (searchEngine) {
                case SearchEngineType.GoogleWeb:
                    return new GoogleResolver(keyword, language, location, path).GetRank();
                   
                default:
                    throw new NotImplementedException();
            }
        }

        public static void UpdateRanks(ApplicationDbContext dbContext) //alternatively, pass connection string
        {
            const SearchEngineType searchEngine = SearchEngineType.GoogleWeb;

            var query = dbContext.Projects
                .Include(p => p.Keywords);

            //Console.WriteLine("Resolver:");

            foreach (var project in query )
            {
                Console.WriteLine("Resolver: " + project.Name);
                var path = project.Paths;
                foreach (var keyword in project.Keywords)
                {
                    Console.WriteLine("-- " + keyword.Value);
                    //GetRank

                    keyword.Ratings = new List<KeywordRating>()
                    {
                        new KeywordRating(GetRank(keyword.Value, keyword.Language, keyword.Location, project.Paths, searchEngine), searchEngine)
                    };
                    dbContext.SaveChanges();
                }
            }

            
            //Dohvaćanje keyworda,Spremajne u model(internal model/PT view model)/direktno zapisivanje nakon GetRank,
            //dbContext.SaveChanges();
        }



        //getKeywords -> object

        //processKeywords

        //updateRanks(dbContext, state)
    }

}
