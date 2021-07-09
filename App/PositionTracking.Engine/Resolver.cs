using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PositionTracking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PositionTracking.Engine
{
    public static class Resolver
    {

        public static Task<int> GetRankAsync(string keyword, Languages language, Countries location, string path, SearchEngineType searchEngine, ILogger logger)
        {
            switch (searchEngine)
            {
                case SearchEngineType.GoogleWeb:
                    return new GoogleResolver(keyword, language, location, path, logger).GetRankAsync();

                default:
                    throw new NotImplementedException();
            }
        }

        private static async Task UpdateKeywordAsync(Keyword keyword, ApplicationDbContext dbContext, ILogger logger)
        {
            const SearchEngineType searchEngine = SearchEngineType.GoogleWeb;

            logger.LogDebug("Writing to database " + keyword.Value);
            keyword.Ratings = new List<KeywordRating>()
            {
                new KeywordRating( await GetRankAsync(keyword.Value, keyword.Language, keyword.Location, keyword.Project.Paths, searchEngine, logger), searchEngine)
            };
            await dbContext.SaveChangesAsync();
        }

        public static void UpdateRanks(ApplicationDbContext dbContext, ILogger logger) //alternatively, pass connection string
        {
            var query = dbContext.Projects
                .Include(p => p.Keywords);

            Task.WaitAll(query.SelectMany(p => p.Keywords).Select(k => UpdateKeywordAsync(k, dbContext, logger)).ToArray());

        }

    }

}
