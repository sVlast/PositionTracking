using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PositionTracking.Models;
using Microsoft.AspNetCore.Authentication;
namespace PositionTracking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            var model = new ProjectsViewModel();
            model.Projects = new ProjectsViewModel.Project[]
            {
                new ProjectsViewModel.Project() {Name="School Work", NumerOfKeywords = 4, Role="Admin"}

            };
            return View(model);
        }

        public IActionResult Keywords()
        {
            var model = new KeywordsViewModel();
            model.ProjectName = "Test 01";
            model.Keywords = new KeywordsViewModel.Keyword[]
            {
                new KeywordsViewModel.Keyword() { Value="Hotels", LanguageLocation="HR-HR", Rating=3},
                new KeywordsViewModel.Keyword() { Value="Pools", LanguageLocation="EN-HR", Rating=5},
                new KeywordsViewModel.Keyword() { Value="Cars", LanguageLocation="DE-DE", Rating=1}
            };

            return View(model);
        }


        public IActionResult Members()
        {
            var model = new MembersViewModel();
            model.ProjectName = "Test02";
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
            var model = new ProjectSettingsViewModel();
            model.ProjectName = "Test 03";
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
