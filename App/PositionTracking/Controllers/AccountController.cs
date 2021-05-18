using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PositionTracking.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PositionTracking.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)

        {
            _signInManager = signInManager;
        }
       

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignUp(string returnUrl)
        {
            return View(new SignUpModel() { ReturnUrl = returnUrl });
        }



        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
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
        public async Task<IActionResult> SignOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }




    }
}