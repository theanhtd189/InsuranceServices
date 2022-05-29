using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Controllers
{
    public class FeedbacksController : Controller
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();
        public IActionResult Index(int page=1,int limit=10)
        {
            if (TempData["msg"] != null)
            {
                ViewBag.Msg = TempData["msg"].ToString();
            }
            var uid = HttpContext.Session.GetInt32("user_id");
            var list = db.Feedbacks
                .Where(x=>x.CustomerId==uid)
                .Include(x => x.Insurance).ThenInclude(x=>x.Category)
                .Include(x => x.Customer)
                .OrderBy(x => x.FeedbackId)

                .ToList();
            var result = list.ToPagedList(page,limit);
            
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)db.Feedbacks.Where(x=>x.CustomerId==uid).ToList().Count/limit);
            return View(result);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Feedback e)
        {
            db.Feedbacks.Add(e);
            var stt = db.SaveChanges()>0;
            if (stt)
            {
                TempData["msg"] = "Save successfully!";
            }
            else
            {
                TempData["msg"] = "Save Failed!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var e = db.Feedbacks.FirstOrDefault(x=>x.FeedbackId==id);
            if(e!=null)
            {
                ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Name",e.InsuranceId);
                return View(e);
            }              
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult Edit(Feedback e)
        {
            var o = db.Feedbacks.FirstOrDefault(x=>x.FeedbackId==e.FeedbackId);
            if (o != null)
            {
                if (o.InsuranceId != e.InsuranceId)
                    o.InsuranceId = e.InsuranceId;
                if(o.FeedbackContent!=e.FeedbackContent)
                    o.FeedbackContent = e.FeedbackContent;
                o.UpdatedAt = DateTime.Now;
                var stt = db.SaveChanges() > 0;
                if (stt)
                {
                    TempData["msg"] = "Save successfully!";
                }
                else
                {
                    TempData["msg"] = "Save Failed!";
                }
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
