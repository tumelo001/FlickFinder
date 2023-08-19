using FlickFinder.Models;
using FlickFinder.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FlickFinder.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult LogIn(string returnUrl)
        {
            return View(new SignInModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(signInModel.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, signInModel.Password, signInModel.RememberMe, false);


                    if (result.Succeeded)
                    {
                        return Redirect(signInModel?.ReturnUrl ?? "/Home/Index");
                    }
                }
            }
            ModelState.AddModelError(" ", "Invalid email or password");
            return View(signInModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Email = registerModel.Email,
                    UserName = registerModel.UserName,
                    AvatarImageURL = registerModel.AvatarImageURL
                };

                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("LogIn");
                }
                else
                {
                    foreach (var error in result.Errors.Select(x => x.Description))
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(registerModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
