using Microsoft.AspNetCore.Mvc;
using Reloj_Control.Models;
using Reloj_Control.Services;
using System;

namespace Reloj_Control.Controllers
{
    public class RelojControlController : Controller
    {
        private DataRelojDAO _dataRelojDAO;

        public RelojControlController()
        {
            _dataRelojDAO = new DataRelojDAO();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckIn()
        {
            // Pass a new instance of the model with the current time set
            var model = new RelojControlCheckInModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult CheckIn(RelojControlCheckInModel model)
        {
            if (ModelState.IsValid)
            {
                // Ensure HoraEntrada is set to the current time
                model.HoraEntrada = DateTime.Now;

                // Save to the database
                if (_dataRelojDAO.RegisterCheckIn(model))
                {
                    // Redirect to the index or any other action as needed
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["FailureMessage"] = "Ya hiciste check in. Para poder hacer check in tienes que hacer check out primero.";
                }
            }

            // If model state is not valid, return the same view with the model to show validation errors
            return View(model);
        }

        public IActionResult CheckOut()
        {
            // Pass a new instance of the model with the current time set
            var model = new RelojControlCheckOutModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult CheckOut(RelojControlCheckOutModel model)
        {
            if (ModelState.IsValid)
            {
                // Ensure HoraSalida is set to the current time
                model.HoraSalida = DateTime.Now;

                // Save to the database
                if (_dataRelojDAO.RegisterCheckOut(model))
                {

                    // Redirect to the index or any other action as needed
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["FailureMessage"] = "No se pudo registrar el check-out. Inténtalo de nuevo.";
                }
            }

            // If model state is not valid, return the same view with the model to show validation errors
            return View(model);
        }
    }
}
