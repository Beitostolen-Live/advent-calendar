using System.Threading.Tasks;
using beitostolen_live_api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace beitostolen_live_api.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return Challenge(
                new AuthenticationProperties { RedirectUri = Url.Page("/Admin") }, "github");
        }

        [HttpGet("signinfacebook")]
        public IActionResult SignInFacebook(int id = 1)
        {
            return Challenge(
                new AuthenticationProperties { RedirectUri = Url.Page($"/Day/{id}") }, "Facebook");
        }

        [HttpGet("signout")]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Page("/SignedOut", pageHandler: null, values: null, protocol: Request.Scheme);
            return SignOut(new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
