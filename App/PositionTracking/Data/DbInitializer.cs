using PositionTracking.Models;
using System;
using System.Linq;

namespace PositionTracking.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            //look for any keywords

            if (context.Projects.Any())
            {
                return;
            }


            var projects = new Project[]
            {
                new Project
                {
                    Name = "Auto dijelovi",
                    Keywords = new Keyword[]
                    {
                        new Keyword
                        {
                            Value = "Auto dijelovi",
                            Entries = new KeywordEntry[]
                            {
                                new KeywordEntry
                                {
                                    Language = "HR",
                                    Location = "HR",
                                    Ratings = new KeywordRating[]
                                    {
                                        new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-5), Rank = 5 },
                                        new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-4), Rank = 8 },
                                        new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-3), Rank = 3 }

                                    }
                                }

                            }
                            
                        }

                    }


                }
            };
        }
    }
}



