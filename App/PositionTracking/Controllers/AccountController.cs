using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PositionTracking.Data;
using PositionTracking.Extensions;
using PositionTracking.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PositionTracking.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EncryptDecryptService _encryptDecryptService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<IdentityUser> signInManager, EncryptDecryptService encryptDecryptService, ApplicationDbContext dbContext,ILogger<AccountController> logger)

        {
            _signInManager = signInManager;
            _signInManager.UserManager.PasswordHasher = new CustomPasswordHasher();
            _encryptDecryptService = encryptDecryptService;
            _dbContext = dbContext;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignUp(string returnUrl, [FromQuery] string t, [FromQuery] string e)
        {
            return View(new SignUpModel()
            {
                ReturnUrl = returnUrl,
                Email = e,
                Token = t
            });
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)

        {
            if (ModelState.IsValid)
                if ((await _signInManager.UserManager.FindByEmailAsync(model.Email)) != null)
                {
                    ModelState.AddModelError(nameof(SignUpModel.Email), $"Email '{model.Email}' is already registered.");
                }

                else
                {
                    string decryptedParam;
                    try
                    {
                        decryptedParam = _encryptDecryptService.DecryptString(model.Token, model.Email);
                    }
                    catch (System.Security.Cryptography.CryptographicException)
                    {
                        _logger.LogError("Invalid Token");
                        ModelState.AddModelError(nameof(SignUpModel.Token), "Invalid Token.");
                        return View(model);
                    }

                    var user = new IdentityUser(model.Email) { Email = model.Email, EmailConfirmed = true };
                    var result = await _signInManager.UserManager.CreateAsync(user, model.Password);

                    if (!result.Succeeded)
                    {
                        ModelState.AddIdentityError(nameof(SignUpModel.Password), result);
                    }
                    else
                    {
                        
                        var param = decryptedParam.Split("|");
                        Guid projectId = Guid.Parse(param[0]);
                        UserRole userRole = Enum.Parse<UserRole>(param[1]);

                         var project = _dbContext.Projects
                        .Where(p => p.ProjectId == projectId)
                        .Include(p => p.UserPermissions)
                        .First();

                        project.AddUserPermission(user,userRole);

                        _dbContext.SaveChanges();
                        
                        return RedirectToAction(nameof(SignIn));
                    }
                }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn(string returnUrl)
        {
            return View(new SignInModel() { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // Checking if url is local prevents redirect attacks. https://docs.microsoft.com/en-us/aspnet/core/security/preventing-open-redirects?view=aspnetcore-2.2
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    else
                        return RedirectToAction("Projects", "Home");
                }
                else
                {
                    ModelState.AddModelError("CustomError", "Invalid email or password.");
                }
            }

            return View(model);

        }

        [HttpPost]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }
    }
}