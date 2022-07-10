using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PositionTracking.Data;
using PositionTracking.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly LanguageDictionary _dictionary;
        //iService collection
        public HomeController(IServiceProvider services)
        {
            _logger = services.GetRequiredService<ILogger<HomeController>>();
            _dbContext = services.GetRequiredService<ApplicationDbContext>();
            _getRankUrl = services.GetRequiredService<IConfiguration>().GetValue<string>("Settings:GetRankUrl");
            _emailSender = services.GetRequiredService<EmailSender>();
            _encryptDecryptService = services.GetRequiredService<EncryptDecryptService>();
            _userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            _dictionary = services.GetRequiredService<LanguageDictionary>();
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

            

            var user = await GetCurrentUserAsync();

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

            return View(new ProjectsViewModel() { Projects = viewProjects, Dictionary = _dictionary });
        }

        public IActionResult KeywordTable(Guid id)
        {
            var viewKeywords = new List<KeywordTableViewModel.Keyword>();

            var project = _dbContext.Projects
                .Where(p => p.ProjectId == id)
                .Include(p => p.Keywords)
                .ThenInclude(k=> k.Ratings)
                .First();


            foreach (var k in project.Keywords)
            {
                var viewRatings = new List<KeywordTableViewModel.KeywordRating>();
                foreach (var r in k.Ratings)
                {
                    viewRatings.Add(new KeywordTableViewModel.KeywordRating()
                    {
                        KeywordRatingId = r.KeywordRatingId,
                        Rank = r.Rank,
                        SearchEngine = SearchEngineType.GoogleWeb,
                        TimeStamp = r.TimeStamp.ToString()
                    }
                    );   
                }

                viewKeywords.Add(new KeywordTableViewModel.Keyword()
                {
                    Id = k.KeywordId.ToString(),
                    Language = k.Language.ToString(),
                    Location = k.Location.ToString(),
                    Value = k.Value,
                });
            };

            return View(new KeywordTableViewModel(project.Name,id) { ProjectId = project.ProjectId, ProjectName = project.Name, keywords = viewKeywords });
        }

        public IActionResult UserProjects(string sortOrder,string searchString)
        {
            ViewData["userSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["projectSortParam"] = sortOrder == "Project" ? "project_desc" : "Project";
            ViewData["permissionSortParam"] = sortOrder == "Permission" ? "permission_desc" : "Permission";
            ViewData["CurrentFilter"] = searchString;


            var permissions = _dbContext.UserPermission.Include(p => p.Project).Include(p => p.User).AsEnumerable();

            if (!String.IsNullOrEmpty(searchString))
            {
                permissions = permissions.Where(p => p.User.UserName.Contains(searchString));
            }

            permissions = sortOrder switch
            {
                "name_desc" => permissions.OrderByDescending(p => p.User.UserName),
                "Project" => permissions.OrderBy(p => p.Project.Name),
                "project_desc" => permissions.OrderByDescending(p => p.Project.Name),
                "Permission" => permissions.OrderBy(p => p.PermissionType),
                "permission_desc" => permissions.OrderByDescending(p => p.PermissionType),
                _ => permissions.OrderBy(p => p.User.UserName),
            };

            var viewUserPermission =
            (from p in permissions
             select new UserProjectsViewModel.UserPermission()
             {
                 Id = p.UserPermissionId,
                 Permission = p.PermissionType.ToString(),
                 Project = p.Project.Name,
                 User = p.User.UserName

             }).ToList();

            return View(new UserProjectsViewModel { UserPermissions = viewUserPermission });
        }
        public async Task<IActionResult> KeywordsAsync(Guid id)
        {
            var user = await GetCurrentUserAsync();

            var userPermission = _dbContext.UserPermission
                .Where(u => u.Project.ProjectId == id & u.User.Id == user.Id)
                .FirstOrDefault();

            if (userPermission == null)
            {
                return RedirectToAction("Index");
            }

                

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

                });
            }

            return View(new KeywordsViewModel(project.Name, project.ProjectId) { Keywords = viewKeywords, GetRankUrl = _getRankUrl });
        }

        [HttpPost]
        public async Task<IActionResult> EditKeyword(Keyword model)
        {
            var keyword = _dbContext.Keywords
                .Where(k => k.KeywordId == model.KeywordId)
                .Include(k=>k.Project)
                .FirstOrDefault();

            if(keyword == null)
            {
                return BadRequest(new {message= "No keyword found!"});
            }

            keyword.Value = model.Value;
            keyword.Language = model.Language;
            keyword.Location = model.Location;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Keywords",new { id = keyword.Project.ProjectId});
        }



            public IActionResult KeywordDetail(Guid id)
        {
            var keyword = _dbContext.Keywords
                .Include(p => p.Project)
                .Where(p => p.KeywordId == id)
                .First();

            var title = keyword.Value + " (" + keyword.Language.ToString() + "-" + keyword.Location.ToString() + ")";
            return View(new KeywordDetailViewModel(keyword.Project.Name, keyword.Project.ProjectId) { Title = title, KeywordId = keyword.KeywordId });
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

            string ImageUrl;

            if (project.ProjectImage != null)
            {
                ImageUrl = "data:image;base64," + Convert.ToBase64String(project.ProjectImage);
            }
            else
            {
                ImageUrl = "";
            }

            return View(new ProjectSettingsViewModel(project.Name, project.ProjectId,ImageUrl) { Domain = project.Paths });
        }
        public IActionResult AccountSettings()
        {
            var user = _dbContext.Users
               .First(u => u.NormalizedEmail == User.Identity.Name.ToUpper());

            return View(new AccountSettingsViewModel() { Email = User.Identity.Name });
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(AddMemberViewModel model)
        {

            var user = _dbContext.Users
                .FirstOrDefault(u => u.NormalizedEmail == model.MemberEmail.ToUpper());

            var project = _dbContext.Projects
            .Where(p => p.ProjectId == model.ProjectId)
            .Include(p => p.UserPermissions)
            .ThenInclude(u => u.User)
            .First();

            if (user != null)
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
                await _emailSender.SendAsync(model.MemberEmail, title, message);
            }

            return RedirectToAction("Members", new { id = model.ProjectId });
        }
        [HttpPost]
        public IActionResult RemoveMember(string memberEmail, Guid projectId)
        {
            //if (_dbContext.Projects.Where(p=> p.ProjectId == projectId).Include(p=>p.UserPermissions).ThenInclude(u=>u.User).Count() == 1)
            //{

            //}

            var project = _dbContext.Projects
            .Where(p => p.ProjectId == projectId)
            .Include(p => p.UserPermissions)
            .ThenInclude(u => u.User)
            .First();


            var permission = project.UserPermissions.First(p => p.User.Email == memberEmail);

            _dbContext.Remove(permission);
            _dbContext.SaveChanges();

            //possible future conflict if User Name value changes from email
            if (User.Identity.Name.Equals(memberEmail, StringComparison.OrdinalIgnoreCase))
                return RedirectToAction(nameof(Projects));
            else
                return RedirectToAction(nameof(Members), new { id = projectId });
        }

        [HttpPost]
        public IActionResult AddKeyword(AddKeywordViewModel model)
        {
            var project = _dbContext.Projects
                    .Where(p => p.ProjectId == model.ProjectId)
                    .First();

            using (Process myProcess = new Process())
                {
            try
            {
                    myProcess.StartInfo.FileName = "dotnet";
                    myProcess.StartInfo.Arguments = $"../PositionTracking.Test/Publish/PositionTracking.Test.dll {model.Value} {project.Paths}";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.StartInfo.UseShellExecute = false;
                    //myProcess.StartInfo.RedirectStandardInput = true;
                    myProcess.StartInfo.RedirectStandardOutput = true;
                    myProcess.StartInfo.RedirectStandardError = true;
                    myProcess.OutputDataReceived += (sender, data) => Console.WriteLine(data.Data);
                    myProcess.ErrorDataReceived += (sender, data) => Console.WriteLine(data.Data);

                    Console.WriteLine("starting process: PositionTracking.Test");
                    myProcess.Start();

                    myProcess.BeginOutputReadLine();
                    myProcess.BeginErrorReadLine();
                    myProcess.WaitForExit();
                    Console.WriteLine($"exit process");
                    int ExitCode = myProcess.ExitCode;
                    if (ExitCode >= 1)
                    {
                        Console.WriteLine($"Testing sucessful - Rank: {ExitCode}");
                    }
                    if(ExitCode == 0) {
                        Console.WriteLine("Testing undetermined: keyword out of range.");
                    }
                    if (ExitCode < 0)
                    {
                        Console.WriteLine("Testing unsucessful.");
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Error while running testing process: {"test"}");
            }
                }

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

        [HttpPost]
        public IActionResult ChangeLanguage(Languages language, string viewPath)
        {
            Response.Cookies.Append("Lang", language.ToString(), new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.UtcNow.AddDays(30) });

            return Redirect(viewPath);
        }

        [HttpPost]
        public async Task<ActionResult> UploadProjectImage(List<IFormFile> files,Guid projectId)
        {
            var project = _dbContext.Projects
                .Where(p => p.ProjectId == projectId)
                .First();
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        formFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        project.ProjectImage = fileBytes;
                        try
                        {
                            var res = await _dbContext.SaveChangesAsync();
                            if (res != 0)
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            return BadRequest(new { count = files.Count, size, projectId });
                        }
                    }

                }
            }
            return RedirectToAction("ProjectSettings",new {id = projectId});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
