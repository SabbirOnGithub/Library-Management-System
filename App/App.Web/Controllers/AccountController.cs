using App.Web.Models.ViewModels;
using App.Web.Providers;
using App.Web.Providers.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace App.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;

        public AccountController(IUserManager userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }


        public IActionResult Login()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Profile()
        {
            return View(this.User.Claims.ToDictionary(x => x.Type, x => x.Value));
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userRepository.Validate(model);

            if (user == null) return View(model);

            await _userManager.SignIn(this.HttpContext, user, false);

            //return LocalRedirect("~/Home/Index");
            return RedirectToAction("Profile");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _userRepository.Register(model);

            await _userManager.SignIn(this.HttpContext, user, false);

            return LocalRedirect("~/Home/Index");
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await _userManager.SignOut(this.HttpContext);
            return RedirectPermanent("~/Account/Login");
        }

    }
}
