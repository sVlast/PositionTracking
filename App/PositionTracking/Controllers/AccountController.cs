using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PositionTracking.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PositionTracking.Controllers
{
    public class AccountController : Controller
    {
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
            return View(model);
        }




    }
}