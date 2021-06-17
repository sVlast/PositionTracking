using System;


namespace PositionTracking.Engine
{
    public static class Resolver
    {
        
        public static int GetRank(string keyword, string language, string location, string path,ResolverType searchEngine)
        {
            switch (searchEngine) {
                case ResolverType.GoogleWeb:
                    return new GoogleResolver(keyword, language, location, path).GetRank();
                   
                default:
                    throw new NotImplementedException();
            }
        }
    }

}
