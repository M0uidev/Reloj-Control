using Microsoft.AspNetCore.Mvc;
using Reloj_Control.Models;
using Reloj_Control.Services;

namespace Reloj_Control.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserModel userModel)
        {
            SecurityService securityService = new SecurityService();

            if (securityService.IsValid(userModel))
            {
                return View("LoginSuccess", userModel);
            }
            else
            {
                ViewData["FailureMessage"] = "Login is incorrect. Try again.";
                return View("Index", userModel);
            }
            
        }
    }
}
