using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ServicesController : Controller
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();
        public IActionResult Index()
        {
            var msg = TempData["msg"]!=null? TempData["msg"].ToString():"";
            if (!string.IsNullOrEmpty(msg))
            {
                ViewBag.Msg = msg;
            }
            ViewBag.list_services = db.Categories.ToList();
            return View();
        }

        public ActionResult Detail(int? cid, int? iid) //cid: category id, iid: insurance id
        {
            if (cid != null)
            {
                var e = db.Categories.FirstOrDefault(x => x.CategoryId == cid);
                if (e != null)
                {
                    if (iid != null)
                    {
                        var o = db.Insurances.FirstOrDefault(x=>x.Id==iid);
                        if (o != null)
                        {
                            return View("IDetail",o);
                        }
                    }
                    ViewBag.list_services = db.Insurances.Where(x => x.CategoryId == cid).ToList();
                    return View("CDetail", e);   
                    
                }               
                TempData["msg"] = "ID không tồn tại!";
                return RedirectToAction("Index");
            }
            else
            if (iid != null)
            {
                var o = db.Insurances.FirstOrDefault(x => x.Id == iid);
                if (o != null)
                {
                    return View("IDetail", o);
                }
            }
            TempData["msg"] = "ID không hợp lệ!";
            return RedirectToAction("Index");    
        }
    }
}
