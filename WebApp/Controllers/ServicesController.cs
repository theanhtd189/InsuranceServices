using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ServicesController : Controller
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int? id)
        {
            return View();
        }
    }
}
