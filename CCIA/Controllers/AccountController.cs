using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CCIA.Models;
using CCIA.Models.AccountViewModels;
using CCIA.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using AspNetCore.Security.CAS;

namespace CCIA.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        // private readonly UserManager<ApplicationUser> _userManager;
        // private readonly SignInManager<ApplicationUser> _signInManager;
        // private readonly IEmailSender _emailSender;
        // private readonly ILogger _logger;
        private readonly CCIAContext _dbContext;

        public AccountController(CCIAContext dbContext)
        {            
            _dbContext = dbContext;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MakePasswords(int id)
        {
            var test = await _dbContext.Contacts.Where(c => c.PasswordHash == null).Take(50).ToListAsync();
            foreach (Contacts cont in test)
            {
                byte[] hashed = KeyDerivation.Pbkdf2(
                    password: cont.Password,
                    salt: cont.Salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);
                cont.PasswordHash = hashed;
                _dbContext.Update(cont);
            }
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [AllowAnonymous]
        public async Task CasLogin(string returnUrl)
        {
            var props = new AuthenticationProperties { RedirectUri = returnUrl };
            await HttpContext.ChallengeAsync(CasDefaults.AuthenticationScheme, props);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync("Cookies");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var test = await _dbContext.Contacts.Where(c => c.EmailAddr == email).ToListAsync();
                foreach (Contacts contact in test)
                {
                    if (VerifyPassword(password, contact))
                    {
                        var claims = new List<Claim>
                        {
                            new Claim("user", email),
                            new Claim("role", "Member"),
                            new Claim("role", "conditioner"),
                            new Claim("contactId", contact.Id.ToString())
                        };

                        await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));
                     

                        if (returnUrl != null)
                        {
                            return LocalRedirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home");

                    }
                }               
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        public bool VerifyPassword(string password, Contacts contact)
        {
            byte[] hashed = KeyDerivation.Pbkdf2(
                    password: password,
                    salt: contact.Salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8);
            if (ByteArraysEqual(hashed, contact.PasswordHash))
            {
                return true;
            }
            return false;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }





        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
