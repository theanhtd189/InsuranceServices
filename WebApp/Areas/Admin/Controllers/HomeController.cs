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

        [HttpGet, Route("Login")]
        public ActionResult Login()
        {
            if (TempData["msg"] != null)
            {
                ViewBag.Msg = TempData["msg"].ToString();
            }
            return View();
        }

        [HttpGet, Route("LogOut")]
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, Route("Setting")]
        public async Task<IActionResult> Setting(WebApp.Models.Admin result)
        {
            var e = db.Admins.FirstOrDefault(x => x.Id == result.Id);
            if (e == null)
            {
                return NotFound();
            }
            else
            {
                if (!string.IsNullOrEmpty(e.Username))
                {
                    if (e.Username != result.Username)
                        e.Username = result.Username;

                    if(!string.IsNullOrEmpty(e.Password))
                    if (e.Password != result.Password)
                        e.Password = result.Password;
                    await db.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Setting));
            }
        }

        [HttpGet, Route("Setting")]
        public ActionResult Setting()
        {
            var uid = HttpContext.Session.GetInt32("admin_id");
            var u = db.Admins.FirstOrDefault(x => x.Id == uid);
            if (u != null)
            {
                return View(u);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost, Route("Login")]
        public ActionResult Login(WebApp.Models.Admin e)
        {
            if (!string.IsNullOrEmpty(e.Username)
                && !string.IsNullOrEmpty(e.Password))
            {
                var o = db.Admins.FirstOrDefault(x => x.Username == e.Username && x.Password == e.Password);
                if (o != null)
                {
                    HttpContext.Session.SetInt32("admin_id", o.Id);
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
