using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MongoPlaces.Core.Contracts.Dto;
using MongoPlaces.Core.Services.Interfaces;
using MongoPlaces.Web.Models;

namespace MongoPlaces.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;

        public AccountController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            //_usersService.CheckCredentials(model.Email, model.Password);
            var result = await _usersService.CheckCredentials(model.Email, model.Password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(model.Email, false);
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }
        

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if ((await _usersService.GetAsync(model.Email)) != null)
                {
                    ModelState.AddModelError("", "The email is already taken.");
                    return View(model);
                }
                await  _usersService.CreateAsync(new UserDto() {Email = model.Email, Password = model.Password});
                FormsAuthentication.SetAuthCookie(model.Email, false);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}