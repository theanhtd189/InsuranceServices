using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();
        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }
    }
}
