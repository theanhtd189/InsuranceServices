using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    public class HomeController : Controller
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet,Route("Login")]
        public ActionResult Login()
        {
            if (TempData["msg"] != null)
            {
                ViewBag.Msg = TempData["msg"].ToString();
            }
            return View();
        }

        [HttpPost, Route("Login")]
        public ActionResult Login(WebApp.Models.Admin e)
        {
            if(!string.IsNullOrEmpty(e.Username) 
                && !string.IsNullOrEmpty(e.Password))
            {
                var o = db.Admins.FirstOrDefault(x=>x.Username==e.Username && x.Password==e.Password);
                if(o!= null)
                {
                    HttpContext.Session.SetInt32("admin_id",o.Id);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = "Username or password is incorrect!";
                }    
            }
            else
            {
                TempData["msg"] = "Invalid username or password!";
                
            }
            return RedirectToAction("Login");
        }
    }
}
