using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace CCIA.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
         public string Message
        {
            set { TempData["Message"] = value; }
        }

        public string EmulationMessage 
        {             
            set => TempData["EmulationMessage"] = value;
        }
        
        private readonly CCIAContext _dbContext;

        public AccountController(CCIAContext dbContext)
        {            
            _dbContext = dbContext;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MakePasswords(int id)
        {
            var test = await _dbContext.Contacts.Where(c => c.PasswordHash == null && c.Password != null && c.Salt != null).Take(500).ToListAsync();
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
            // TODO: Check returnURL, use if not blank
            var props = new AuthenticationProperties { RedirectUri = "/Admin/AdminHome/Index" };
            await HttpContext.ChallengeAsync("CAS", props);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = "/client/home/")
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync("Cookies");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = "/client/home/")
        {
            if(returnUrl.Contains("admin"))
            {
                returnUrl = "/client/home/";
            }
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var test = await _dbContext.Contacts.Where(c => c.Email == email).ToListAsync();
                foreach (Contacts contact in test)
                {
                    if (VerifyPassword(password, contact))
                    {
                        await CompleteSignin(contact, false);

                        if (returnUrl != null)
                        {
                            return LocalRedirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home", new { area = "Client" });

                    }
                }               
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }

        public async Task CompleteSignin(Contacts contact, bool isEmulation)
        {
            var claims = new List<Claim>
            {
                new Claim("user", contact.Email),
                new Claim("role", "Member"),
                new Claim("role", "conditioner"),
                new Claim("contactId", contact.Id.ToString()),
                new Claim("orgId", contact.OrgId.ToString()),               
            };
            if(isEmulation)
            {
                claims.Add(new Claim("role", "Emulated"));
            }

            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

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


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction(nameof(Login));
        }      
        
        
         public IActionResult Denied()
        {
            return View();
        }

        [Authorize(Roles = "CoreStaff")] 
        public async Task<RedirectToActionResult> Emulate(int id /* Login ID*/)
        {
            if (id != 0)
            {
                var contact = await _dbContext.Contacts.Where(c => c.Id == id).FirstOrDefaultAsync();
                if(contact == null)
                {
                    Message = "Contact not found";
                   return RedirectToAction("Index", "AdminHome", new { Area = "Admin"});
                }
                
                EmulationMessage = string.Format("Emulating {0}.  <a class='btn btn-dark' href='/Account/EndEmulate'>Exit</a>", contact.Name);
                await CompleteSignin(contact, true);
                return RedirectToAction("Index", "Home", new { Area = "Client"});
            }
            else
            {
                Message = "Login ID not provided.  Use /Emulate/login";
            }
            
            return RedirectToAction("Index", "AdminHome", new { Area = "Admin"});
        }

        public async Task<RedirectToActionResult> EndEmulate()
        {
            await HttpContext.SignOutAsync("Cookies");
            EmulationMessage = "";

            return RedirectToAction("CasLogin");
        }

        
    }
}
