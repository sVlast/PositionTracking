using System;
using PositionTracking.Data;

namespace PositionTracking.Engine
{
    public static class Resolver
    {
        
        public static int GetRank(string keyword, Languages language, Countries location, string path,ResolverType searchEngine)
        {
            switch (searchEngine) {
                case ResolverType.GoogleWeb:
                    return new GoogleResolver(keyword, language, location, path).GetRank();
                   
                default:
                    throw new NotImplementedException();
            }
        }

        public static void UpdateRanks(ApplicationDbContext dbContext) //connection string
        {
            //Dohvaćanje keyworda,Spremajne u model(internal model/PT view model)/direktno zapisivanje nakon GetRank,
            dbContext.SaveChanges();
        }

        //getKeywords -> object

        //processKeywords

        //updateRanks(dbContext, state)
    }

}
