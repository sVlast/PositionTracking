using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PositionTracking.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PositionTracking.Engine
{
    public static class Resolver
    {
        
        public static Task<int> GetRankAsync(string keyword, Languages language, Countries location, string path,SearchEngineType searchEngine,ILogger logger)
        {
            switch (searchEngine) {
                case SearchEngineType.GoogleWeb:
                    using (var r = new GoogleResolver(keyword, language, location, path, logger))
                        return r.GetRankAsync();


                default:
                    throw new NotImplementedException();
            }
        }

        public static async void UpdateRanks(ApplicationDbContext dbContext,ILogger logger) //alternatively, pass connection string
        {
            const SearchEngineType searchEngine = SearchEngineType.GoogleWeb;

            var query = dbContext.Projects
                .Include(p => p.Keywords);

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
                        new KeywordRating( await GetRankAsync(keyword.Value, keyword.Language, keyword.Location, project.Paths, searchEngine,logger), searchEngine)
                    };
                    await dbContext.SaveChangesAsync();
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
