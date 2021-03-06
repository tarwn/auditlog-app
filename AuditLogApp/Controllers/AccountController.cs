﻿using AuditLogApp.Common.Enums;
using AuditLogApp.Membership;
using AuditLogApp.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AuditLogApp.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private static string[] ACCEPTABLE_CODES = new string[] {
            "SUMMER2018"
        };

        private IUserMembership _membership;

        public AccountController(IUserMembership membership)
        {
            _membership = membership;
        }

        [HttpGet("register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPostAsync(RegisterModel model)
        {
            //TEMPORARY - restrict registration to pilot code holders
            if (!IsValidPilotCode(model.PilotCode))
            {
                ModelState.AddModelError("PilotCode", "AuditLog is currently in closed pilot. Email support@auditlog.co if you don't have one!");
            }

            if (!ModelState.IsValid)
            {
                return View("Register");
            }

            var result = await _membership.RegisterAsync(model.CompanyName, model.UserName, model.Email, model.Password);
            if (result.Failed)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View("Register", model);
            }

            return LocalRedirect(_membership.Options.DefaultPathAfterLogin);
        }

        [HttpGet("register/twitter")]
        [AllowAnonymous]
        public IActionResult RegisterWithTwitter()
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = "/account/register/twitter/continue"
            };
            return Challenge(props, "Twitter");
        }

        [HttpGet("register/twitter/continue")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterWithTwitterContinueAsync()
        {
            // use twitter info to set some sensible defaults
            var cookie = await HttpContext.AuthenticateAsync("ExternalProvidersCookie");
            var twitterId = cookie.Principal.FindFirst("urn:twitter:userid");
            var twitterUsername = cookie.Principal.FindFirst("urn:twitter:screenname");

            // verify the id is not already registered, short circuit to login screen
            if (await _membership.IsAlreadyRegisteredAsync(CredentialType.Twitter, twitterId.Value))
            {
                ModelState.AddModelError("", $"Welcome back! Your twitter account @{twitterUsername.Value} is already registered. Maybe login instead?");
                return View("Login");
            }

            var suggestedUsername = await FindUniqueSuggestionAsync(twitterUsername.Value);

            var model = new RegisterWithTwitterModel()
            {
                TwitterId = twitterId.Value,
                TwitterUsername = twitterUsername.Value,
                UserName = suggestedUsername
            };

            return View("RegisterWithTwitterContinue", model);
        }

        private async Task<string> FindUniqueSuggestionAsync(string startingName, int numAttempts = 5)
        {
            string suggestedUsername = startingName;
            var rand = new Random();
            for (int attemptCount = 0; attemptCount < numAttempts; attemptCount++)
            {
                if (await _membership.IsUsernameAvailable(suggestedUsername))
                {
                    return suggestedUsername;
                }
                else
                {
                    suggestedUsername = $"{startingName}{Math.Floor(rand.NextDouble() * 10000)}";
                }
            }
            return "";
        }

        [HttpPost("register/twitter/continue")]
        public async Task<IActionResult> RegisterWithTwitterContinueAsync(RegisterWithTwitterModel model)
        {
            //TEMPORARY - restrict registration to pilot code holders
            if (!IsValidPilotCode(model.PilotCode))
            {
                ModelState.AddModelError("PilotCode", "AuditLog is currently in closed pilot. Email support@auditlog.co if you don't have one!");
            }

            if (!ModelState.IsValid)
            {
                return View("RegisterWithTwitterContinue", model);
            }

            var result = await _membership.RegisterExternalAsync(model.CompanyName, model.UserName, model.Email, CredentialType.Twitter, model.TwitterId, $"Twitter: {model.TwitterUsername}");
            if (result.Failed)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View("RegisterWithTwitterContinue", model);
            }

            await HttpContext.SignOutAsync("ExternalProvidersCookie");

            return LocalRedirect(_membership.Options.DefaultPathAfterLogin);
        }

        private bool IsValidPilotCode(string pilotCode)
        {
            if (String.IsNullOrEmpty(pilotCode)) {
                return false;
            }

            return ACCEPTABLE_CODES.Any(a => a.Equals(pilotCode, StringComparison.InvariantCultureIgnoreCase));
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string returnUrl = null)
        {
            // ensure we're starting w/ a clean slate
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("ExternalProvidersCookie");

            TempData["returnUrl"] = returnUrl;
            return View("Login");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPostAsync(LoginModel user, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View("Login", user);
            }

            var result = await _membership.LoginAsync(user.UserName, user.Password);
            if (result.Failed)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View("Login", user);
            }

            return LocalRedirect(returnUrl ?? _membership.Options.DefaultPathAfterLogin);
        }

        [HttpGet("login/twitter")]
        [AllowAnonymous]
        public IActionResult LoginWithTwitter(string returnUrl = null)
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = "/account/login/twitter/continue?returnUrl=" + HttpUtility.UrlEncode(returnUrl)
            };
            return Challenge(props, "Twitter");
        }

        [HttpGet("login/twitter/continue")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithTwitterContinueAsync(string returnUrl = null)
        {
            // use twitter info to create a session
            var cookie = await HttpContext.AuthenticateAsync("ExternalProvidersCookie");
            var twitterId = cookie.Principal.FindFirst("urn:twitter:userid");

            var result = await _membership.LoginExternalAsync(CredentialType.Twitter, twitterId.Value);
            if (result.Failed)
            {
                ModelState.AddModelError("", "Twitter account not recognized, have you registered yet?");
                return View("Login");
            }

            await HttpContext.SignOutAsync("ExternalProvidersCookie");

            return LocalRedirect(returnUrl ?? _membership.Options.DefaultPathAfterLogin);
        }


        [HttpGet("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> LogoutAsync()
        {
            await _membership.LogoutAsync();

            if (_membership.Options.DefaultPathAfterLogout != null)
            {
                return Redirect(_membership.Options.DefaultPathAfterLogout);
            }
            else
            {
                return RedirectToAction("LoginAsync");
            }
        }
    }
}
