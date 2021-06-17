using Microsoft.AspNetCore.Identity;
using PositionTracking.Models;
using System;
using System.Linq;

namespace PositionTracking.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider services)
        {

            var context = (ApplicationDbContext)services.GetService(typeof(ApplicationDbContext));
            var signInManager = (SignInManager<IdentityUser>)services.GetService(typeof(SignInManager<IdentityUser>));
            //look for any keywords

            if (!context.Users.Any())
            {
                var result = signInManager.UserManager.CreateAsync(new IdentityUser
                {
                    UserName = "dino@position.com",
                    Email = "dino@position.com",
                    EmailConfirmed = true
                }, "1234").Result;

                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("User creation failed. " + result.ToString());

                result = signInManager.UserManager.CreateAsync(new IdentityUser
                {
                    UserName = "sandro@position.com",
                    Email = "sandro@position.com",
                    EmailConfirmed = true
                }, "5678").Result;

            }

            if (context.Projects.Any())
            {
                return;
            }

            var dino = signInManager.UserManager.FindByEmailAsync("dino@position.com").Result;
            var sandro = signInManager.UserManager.FindByEmailAsync("sandro@position.com").Result;

            context.Projects.Add(new Project(dino, UserPermission.Admin)
            {
                Name = "Auto dijelovi",
                Keywords = new Keyword[]
                    {
                        new Keyword
                        {
                            Value = "Auto dijelovi",
                            Language = "HR",
                            Location = "HR",
                            Ratings = new KeywordRating[]
                            {
                                new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-5), Rank = 5 },
                                new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-4), Rank = 8 },
                                new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-3), Rank = 3 }

                            }
                        },

                        new Keyword
                        {
                            Value= "Car parts",
                            Language = "EN",
                            Location = "HR",
                            Ratings = new KeywordRating[]
                            {
                                new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-4), Rank = 4 },
                                new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-3), Rank = 3 },
                                new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-1), Rank = 2 }
                            }
                        }
                    }

            });
            context.SaveChanges();


            context.Projects.Add(new Project(sandro, UserPermission.Admin)
            {
                Name = "Ljetovanje",
                Keywords = new Keyword[]
                    {
                        new Keyword
                        {
                            Value="Ljetovanje",
                            Language = "HR",
                            Location = "HR",
                            Ratings = new KeywordRating[]
                            {
                                new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-4), Rank = 5 },
                                new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-2), Rank = 6 },
                                new KeywordRating { SearchEngine = "Google", TimeStamp = DateTime.Now.AddDays(-1), Rank = 7 }

                            }
                        }
                    }
            });

            context.SaveChanges();

        }
    }
}



