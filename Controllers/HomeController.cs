using Microsoft.AspNetCore.Mvc;
using PracticalProject.Interface_and_repos;
using PracticalProject.Models;
using System.Diagnostics;

namespace PracticalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPatientTransactions _patientTransactions;
        private static string message = string.Empty;

        public HomeController(ILogger<HomeController> logger, IPatientTransactions invoice)
        {
            _logger = logger;
            this._patientTransactions = invoice;
            //Items = new List<InvItems>();
        }

        public IActionResult UpdatePatientDetials(string id)
        {
            var invoicemodel = _patientTransactions.EditPatientById(id);
            ViewBag.msg = message;
            message = string.Empty;
            return View(invoicemodel);
        }
        [HttpGet]
        public IActionResult PatientList()
        {
            HomeViewModel homeViewModel = new HomeViewModel();

            var products = _patientTransactions.GetPatients();

            return View(products);
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            return View(homeViewModel);
        }


        /// <summary>
        /// Edit invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditPatient(string id)
        {
            HomeViewModel patient = new HomeViewModel();
            patient = (HomeViewModel)_patientTransactions.GetPatientById(id);
            return View(patient);
        }

        /// <summary>
        /// Edit invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddPatient(HomeViewModel model)
        {
            model.Id = Guid.NewGuid();
            var resp= _patientTransactions.AddPatient(model);
            return Json(resp);
        }


        [HttpPost]
        public IActionResult UpdatePatient(HomeViewModel model)
        {
            var message = "Failed to update patient details.";
            if (ModelState.IsValid)
            {
                var updated = _patientTransactions.UpdatePatientById(model);

                message = updated != null ? "Patient Details have been updated succesfully." : message;
            }

            return Json(message);
        }

      

        public IActionResult ViewInsuranceDetails(string insuranceId)
        {
            var patientDetails = _patientTransactions.GetPatientById(insuranceId);
            var myinvoice = _patientTransactions.GeneratePDF(patientDetails);
            return myinvoice;
        }


        public IActionResult RemovePatient(string insuranceId)
        {
            message = "Failed to remove patient.";
            var updated = _patientTransactions.RemovePatient(insuranceId);
            message = updated == true ? "Patient data removed." : message;
            return RedirectToAction("PatientList");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}