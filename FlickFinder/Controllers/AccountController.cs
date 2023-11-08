using FlickFinder.Models;
using FlickFinder.Models.ViewModels;
using FlickFinder.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace FlickFinder.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;

        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 IMailService mailService,
                                 IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _config = config;
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
                    try
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        Dictionary<string, string> emailOptions = new Dictionary<string, string>
                        {
                            { "username", user.UserName },
                            { "link",Request.Host.Value + $"/Account/VerifyEmail?userId={user.Id}&token={HttpUtility.UrlEncode(token)}" }
                        };
                        await _mailService.SendMailAsync(registerModel.Email, "Welcome to FlickFinder", "WelcomeEmail", emailOptions);
                    }
                    catch (Exception) { }

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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    try
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        Dictionary<string, string> emailOptions = new Dictionary<string, string>
                        {
                            { "username", user.UserName },
                            { "link", Request.Host.Value + $"/Account/ResetPassword?userId={user.Id}&token={HttpUtility.UrlEncode(token)}" }
                        };
                        await _mailService.SendMailAsync(user.Email, "Reset Password", "ForgotPassword", emailOptions);
                        TempData["Message"] = $"Reset email was sent to {user.Email}";
                        return View();
                    }
                    catch (Exception)
                    {}

                }
                ModelState.AddModelError("", "Invalid. Email not regestered");
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPassword() { Token = token, UserId = userId });
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.ConfirmPassword);
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Successfully reset password";
                        return RedirectToAction("LogIn");
                    }
                }
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult VerifyEmail(string userId, string token)
        {
            return View(new ConfirmEmail { UserId = userId, Token = token });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(ConfirmEmail model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user.EmailConfirmed)
                {
                    TempData["Message"] = "Email already been verified";
                    return RedirectToAction("LogIn");
                }
                if (user != null)
                {
                    var token = model.Token;
                    var result = await _userManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "Email verified successfully";
                        return RedirectToAction("LogIn");
                    }
                    TempData["Message"] = "Something went wrong😐.";
                }
            }
            return View(model);
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
