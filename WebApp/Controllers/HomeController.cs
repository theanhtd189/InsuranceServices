using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApp.Models;

namespace WebApp.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private InsuranceOnlineContext db = new InsuranceOnlineContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"];
            }
            ViewBag.list_feedbacks = db.Feedbacks.ToList();
            ViewBag.list_services = db.Categories.ToList();
            return View();
        }

        [Route("Our-Company")]
        public ActionResult OurCompany()
        {
            return View();
        }


        [Route("Our-Team")]
        public ActionResult OurTeam()
        {
            return View();
        }

        [Route("Contact")]
        public ActionResult Contact()
        {
            return View();
        }


        [Route("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}