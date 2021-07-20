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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System.Web;

namespace PositionTracking.Controllers

{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly string _getRankUrl;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly EmailSender _emailSender;
        private readonly EncryptDecryptService _encryptDecryptService;
        private readonly UserManager<IdentityUser> _userManager;
        //iService collection
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration,EmailSender emailSender,EncryptDecryptService encryptDecryptService,UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _dbContext = context;
            _getRankUrl = configuration.GetValue<string>("Settings:GetRankUrl");
            _emailSender = emailSender;
            _encryptDecryptService = encryptDecryptService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private Task<IdentityUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User);
        }

        public async Task<IActionResult> Projects()
        {
            var user =await GetCurrentUserAsync();

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
                    LanguageLocation = k.Language.ToString() + '-' + k.Location.ToString(),
                    Id = k.KeywordId.ToString(),
                    Rating = k.Ratings.FirstOrDefault()?.Rank ?? 0,

                }); ;
            }

            return View(new KeywordsViewModel(project.Name, project.ProjectId) { Keywords = viewKeywords, GetRankUrl = _getRankUrl });
        }

        public IActionResult Members(Guid id)
        {
            var project = _dbContext.Projects
                .Where(p => p.ProjectId == id)
                .Include(p => p.UserPermissions)
                .ThenInclude(u => u.User)
                .First();

            var viewMembers = new List<MembersViewModel.Member>();

            foreach (var u in project.UserPermissions)
            {
                viewMembers.Add(new MembersViewModel.Member()
                {
                    MemberName = u.User.Email,
                    Email = u.User.Email,
                    PermissionType = u.PermissionType.ToString()
                });
            }

            return View(new MembersViewModel(project.Name, project.ProjectId) { Members = viewMembers });
        }

        public IActionResult ProjectSettings(Guid id)
        {
            var project = _dbContext.Projects
            .Where(p => p.ProjectId == id)
            .First();

            return View(new ProjectSettingsViewModel(project.Name, project.ProjectId) { Domain = project.Paths });
        }
        public IActionResult AccountSettings()
        {
            var user = _dbContext.Users
               .First(u => u.NormalizedEmail == User.Identity.Name.ToUpper());

            return View(new AccountSettingsViewModel() { Email = User.Identity.Name });
        }

        

        [HttpPost]
        //metoda addMemeber - Project, MemberAccessException, Role
        public async Task<IActionResult> AddMember(AddMemberViewModel model)
        {
            
            var user = _dbContext.Users
                .FirstOrDefault(u => u.NormalizedEmail == model.MemberEmail.ToUpper());

            var project = _dbContext.Projects
            .Where(p => p.ProjectId == model.ProjectId)
            .Include(p => p.UserPermissions)
            .ThenInclude(u=>u.User)
            .First();

            if(user != null)
            {
                var permission = project.UserPermissions.FirstOrDefault(u => u.User == user);
                if (permission == null)
                {
                    project.AddUserPermission(user, model.UserRole);
                }
                else
                {
                    permission.PermissionType = model.UserRole;
                }
                _dbContext.SaveChanges();
                
            }
            else
            {
                user = await GetCurrentUserAsync();
                var encryptedLinkParam = _encryptDecryptService.EncryptString($"{project.ProjectId}|{(int)model.UserRole}", model.MemberEmail);
                var message = $"Hello, \n" +
                    $"you have been invited to participate in managing position tracking for {project.Name} by {user.UserName} \n" +
                    $"Before you can acces it you need to sign-up by clicking on the following link: \n" +
                    $"https://localhost:5001/account/signup?t={ HttpUtility.UrlEncode(encryptedLinkParam) }&&e={HttpUtility.UrlEncode(model.MemberEmail)} \n";
                var title = $"Invitation to {project.Name} on Position Tracking from {user.UserName}";
                await _emailSender.SendAsync(model.MemberEmail,title,message);
            }

            //da li postoji user s ovim mailom
            //ako ne postoji generate link, query string encrypt project ID
            //sign up metoda
            //klik na link ode na stranicu - projecId ,role Enum, email - enkriptiran
            //UserManager
            //ako postoji nađi projekt ID include user permisson
            //add user and role
            //return view members

            return RedirectToAction("Members", new { id = model.ProjectId});
        }

        [HttpPost]
        public IActionResult AddKeyword(AddKeywordViewModel model)
        {
            var project = _dbContext.Projects
                .Where(p => p.ProjectId == model.ProjectId)
                .First();

            project.Keywords = new List<Keyword>()
            { new Keyword()
                {
                    Value = model.Value,
                    Language = model.Language,
                    Location = model.Location
                }
            };

            _dbContext.SaveChanges();

            return RedirectToAction("Keywords", new { id = model.ProjectId });  //dynamic object
        }

        [HttpPost]
        public IActionResult DeleteKeyword(Guid id)
        {
            var keyword = _dbContext.Keywords
                .Where(k => k.KeywordId == id)
                .Include(k => k.Ratings)
                .Include(k => k.Project)
                .First();

            _dbContext.Remove(keyword);
            _dbContext.RemoveRange(keyword.Ratings);

            _dbContext.SaveChanges();
            return RedirectToAction("Keywords", new { id = keyword.Project.ProjectId });
        }

        [HttpPost]
        public IActionResult AddProject(AddProjectViewModel model)
        {
            var user = _dbContext.Users
                .First(u => u.NormalizedEmail == User.Identity.Name.ToUpper());

            _dbContext.Projects.Add(new Project(user, UserRole.Admin)
            {
                Name = model.ProjectName,
                Paths = model.Domain
            });

            _dbContext.SaveChanges();

            return RedirectToAction("Projects");  //dynamic object
        }

        [HttpPost]
        public IActionResult DeleteProject(Guid id)
        {
            var project = _dbContext.Projects
                .Where(p => p.ProjectId == id)
                .Include(p => p.Keywords)
                .ThenInclude(k => k.Ratings)
                .Include(k => k.UserPermissions)
                .First();

            _dbContext.Remove(project);
            _dbContext.RemoveRange(project.UserPermissions);
            _dbContext.RemoveRange(project.Keywords);
            _dbContext.RemoveRange(project.Keywords.SelectMany(k => k.Ratings));
            _dbContext.SaveChanges();

            return RedirectToAction("Projects");
        }

        [HttpPost]
        public IActionResult EditProject(ProjectSettingsViewModel model)
        {
            var project = _dbContext.Projects
                .Where(p => p.ProjectId == model.ProjectId)
                .First();

            project.Name = model.ProjectName;
            project.Paths = model.Domain;

            _dbContext.SaveChanges();

            return View("ProjectSettings", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
