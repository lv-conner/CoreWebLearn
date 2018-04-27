using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CookieAuthentication.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            var userId = HttpContext.User.Identity.Name;
            return Content(JsonConvert.SerializeObject(HttpContext.User));
        }


        public async Task<IActionResult> Login()
        {
            var property = new AuthenticationProperties()
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(2),
                IsPersistent = true,
            };
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Email, "234123@qq.com"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "Tim"));
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(user, property);
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return View();
        }


        public IActionResult AfterLogout()
        {
            return View();
        }
    }
}