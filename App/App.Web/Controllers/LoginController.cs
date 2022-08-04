using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
