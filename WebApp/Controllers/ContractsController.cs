using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Controllers
{
    public class ContractsController : Controller
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();
        public IActionResult Index(int? page)
        {
            
            var uid = HttpContext.Session.GetInt32("user_id");
            var u = db.Customers.FirstOrDefault(x => x.Id == uid);
            if (u != null)
            {
                var pageNumber = page ?? 1;
                var list_contract = db.Contracts.Where(x => x.CustomerId == uid).Include(x => x.Insurance.Category).Include(x => x.Insurance).Include(x => x.Payments).OrderBy(x=>x.Id).ToList();
                var list = list_contract.ToPagedList(pageNumber,6);
                ViewBag.List_Contract = list;
                ViewBag.Page = pageNumber;
                ViewBag.TotalPage = Math.Ceiling((double)list_contract.Count/6);
                return View();
            }
            else
            {
                return NotFound();
            }               
        }

        public ActionResult Detail(int id)
        {
            if(TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"];
            }
            var o = db.Contracts.FirstOrDefault(x => x.Id == id);
            if (o != null)
            {
                return View(o);
            }
            else
            {
                return RedirectToAction("", "Home");
            }
        }

        [HttpGet]
        public ActionResult Cancel(int id)
        {
            var o = db.Contracts.FirstOrDefault(x => x.Id == id);
            if (o != null)
            {
                o.Status = null;
                var stt = db.SaveChanges() > 0;
                if (stt)
                {
                    TempData["Msg"] = "Canceled the contract successfully!";
                }
                else
                {
                    TempData["Msg"] = "Error!";
                }                                  
            }
            else
            {
                TempData["Msg"] = "Error!";
                
            }
            return RedirectToAction("","Home");
        }

        [HttpGet]
        public ActionResult Pay(int id)
        {
            var o = db.Contracts.FirstOrDefault(x => x.Id == id);
            if (o != null)
            {
                var created_payment = db.Payments.FirstOrDefault(x=>x.ContractId==id);
                if (created_payment == null)
                {
                    var payment = new Payment
                    {
                        ContractId = id,
                        Total=0
                    };
                    db.Payments.Add(payment);
                    var stt = db.SaveChanges()>0;
                    if(!stt)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        created_payment = payment;
                    }    
                }
                return View("Payment", created_payment);
            }
            else
            {
                return RedirectToAction("", "Home");
            }
        }

        [HttpPost]
        public ActionResult Pay(int ContractID, int PaymentID, decimal PaidAmount, string ContentDetails, int PeriodicID)
        {
            var o = db.Payments.FirstOrDefault(x => x.Id == PaymentID);
            if (o != null)
            {
                PaymentDetail e = new PaymentDetail
                {
                    ContentDetails = HttpUtility.UrlEncode(ContentDetails),
                    CreatedAt = DateTime.Now,
                    PaidAmount= PaidAmount,
                    PaymentId=PaymentID,
                    PeriodicId=PeriodicID
                };
                db.PaymentDetails.Add(e);
                var stt = db.SaveChanges() > 0;
                if (stt)
                {
                    var init = o.Total;
                    o.Total = init + e.PaidAmount;
                    db.SaveChanges();
                    TempData["Msg"] = "Paid successfully!";
                }
                else
                {
                    TempData["Msg"] = "Error";
                }
                return RedirectToAction("Detail","Contracts", new {id=ContractID});
            }
            else
            {
                return RedirectToAction("", "Home");
            }
        }
    }
}
