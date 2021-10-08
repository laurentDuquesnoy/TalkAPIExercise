using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalkAPIExercise.Model;

namespace TalkAPIExercise.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        
        
        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> SignIn(string returnUrl)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var model = new SignInModel
            {
                ReturnUrl = returnUrl
            };
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(signInModel.Email, signInModel.Password, signInModel.RememberMe , lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToReturnUrl(signInModel.ReturnUrl);
                }
            }

            return View(signInModel);
        }

        [HttpGet]
        public async Task<IActionResult> Register(string returnUrl)
        {
            return View(new RegisterModel {ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser {UserName = model.Email, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToReturnUrl(model.ReturnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SignOut(string returnUrl)
        {
            await _signInManager.SignOutAsync();
            return returnUrl != null ? RedirectToReturnUrl(returnUrl) : RedirectToAction("Index", "Home");
        }
        
        private IActionResult RedirectToReturnUrl(string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "/";
            }

            return LocalRedirect(returnUrl);
        }
    }
}