using Microsoft.AspNetCore.Mvc;
using Reloj_Control.Models;
using Reloj_Control.Services;

namespace Reloj_Control.Controllers
{
    public class RegisterController : Controller
    {
        private UsersDAO _usersDAO;

        public RegisterController()
        {
            _usersDAO = new UsersDAO();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserModel userModel)
        {
            try
            {
                if (_usersDAO.Register(userModel))
                {
                    return View("RegisterSuccess", userModel);
                }
                else
                {
                    ViewData["FailureMessage"] = "Ese nombre de usuario ya existe. Intenta con otro.";
                    return View("Index", userModel);
                }
            }
            catch (Exception e)
            {
                ViewData["FailureMessage"] = e.Message;
                return View("Index", userModel);
            }
        }
    }
}
