using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PositionTracking.Models;
using Microsoft.AspNetCore.Authentication;
using PositionTracking.Data;
using Microsoft.EntityFrameworkCore;

namespace PositionTracking.Controllers

{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Projects()
        {
            var user = _dbContext.Users
                .First(u => u.NormalizedEmail == User.Identity.Name.ToUpper());

            var permissions = _dbContext.Projects
                .SelectMany(p => p.UserPermissions)
                .Where(up => up.User == user)
                .Include(up => up.Project)
                .ThenInclude(p => p.Keywords);
            

            var viewProjects = new List<ProjectsViewModel.Project>();

            foreach (var p in permissions)
            {
                viewProjects.Add(new ProjectsViewModel.Project()
                {
                    Name = p.Project.Name,
                    NumerOfKeywords = p.Project.Keywords.Count,
                    Role = p.PermissionType.ToString(),
                    Id = p.Project.ProjectId
                });
            }

            return View(new ProjectsViewModel() { Projects = viewProjects });
        }

        public IActionResult Keywords(Guid id)
        {
            var project = _dbContext.Projects
                .Where(p => p.ProjectId == id)
                .Include(p => p.Keywords)
                .ThenInclude(k => k.Ratings.OrderByDescending(r => r.TimeStamp).Take(1))
                .First();


            var viewKeywords = new List<KeywordsViewModel.Keyword>();

            foreach (var k in project.Keywords)
            {
                viewKeywords.Add(new KeywordsViewModel.Keyword()
                {
                    Value = k.Value,
                    LanguageLocation = k.Language + '-' + k.Location,
                    Rating = k.Ratings.FirstOrDefault()?.Rank ?? 0
                   
                }); ;
            }


            return View(new KeywordsViewModel(project.Name) { Keywords = viewKeywords });
        }


        //var permission = _dbContext.Model

        //    var viewKeywords = new List<KeywordsViewModel.Keyword>()




        public IActionResult Members()
        {

            var model = new MembersViewModel("Pro");
            model.Members = new MembersViewModel.Member[]
            {
                new MembersViewModel.Member() {MemberName="Mihovil",Email="mihovil@miho.com",PermissionType="Admin"},
                new MembersViewModel.Member() {MemberName="Ivan",Email="hrvoje@miho.com",PermissionType="Edit"},
                new MembersViewModel.Member() {MemberName="Hrvoje",Email="ivan@miho.com",PermissionType="View"}
            };
            return View(model);
        }

        public IActionResult ProjectSettings()
        {
            var model = new ProjectSettingsViewModel("bla");
                

            model.Domain = "https://www.example.com";



            return View(model);
        }
        public IActionResult AccountSettings()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
