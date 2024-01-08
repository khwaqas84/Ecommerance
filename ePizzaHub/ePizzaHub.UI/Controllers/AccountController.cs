using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace ePizzaHub.UI.Controllers
{
    public class AccountController : Controller
    {
        IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model, string returnUrl )
        {
            if(ModelState.IsValid)
            {
                UserModel user = _authService.ValidateUser(model.Email, model.Password);
                if(user != null)
                {
                    GenerateTicket(user);
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                    return Redirect(returnUrl);
                    }
                   else if (user.Roles.Contains("User"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "User" });
                    }
                    else if (user.Roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Login", "Account");
        }

        private async void GenerateTicket(UserModel user)
        {
            string strData = JsonSerializer.Serialize(user);
            var claims = new List<Claim> 
            { 
            new Claim(ClaimTypes.UserData, strData),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, string.Join(',',user.Roles))
            };

            var identity= new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme
                    , new ClaimsPrincipal(identity)
                    , new AuthenticationProperties { AllowRefresh=true,ExpiresUtc=DateTime.UtcNow.AddMinutes(60) }
                    );
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult UnAuthorize()
        {
            return View();
        }
    }
}
