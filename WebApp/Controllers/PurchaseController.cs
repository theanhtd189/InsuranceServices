using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class PurchaseController : Controller 
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();

        [HttpGet,Route("Purchase/GetService/{id}")]
        public ActionResult Index(int? id)
        {
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"];
            }
            if (id == null)
            {
                return RedirectToAction("", "Home");
            }
            else
            {
                ViewBag.ContinueID = id;
                if (id > 0)
                {
                    var o = db.Insurances.FirstOrDefault(x => x.Id == id);
                    if (o != null)
                    {
                        return View("Create",o);
                    }
                }
                return RedirectToAction("", "Services");
            }
        }

        [HttpPost]
        public ActionResult Process(int InsuranceID, int CustomerID, string Created_at, string Exprired_at,int duration, decimal Total)
        {
            var e = new Contract
            {
                CreatedAt = DateTime.Parse(Created_at),
                ExpriredAt = DateTime.Parse(Exprired_at),
                Status =false,
                CustomerId= CustomerID,
                InsuranceId=InsuranceID,
                Duration=duration,
                Total=Total
            };
            db.Contracts.Add(e);
            var stt =  db.SaveChanges() > 0;
            if (stt)
            {
                TempData["Msg"] = "Make a contract successfully!";
                return RedirectToAction("Detail","Contracts",new {id=e.Id});
            }
            else
            {
                var back_url = HttpContext.Request.Path.ToString();
                TempData["Msg"] = "Error!";
                return Redirect(back_url);
            }
            
        }
    }
}
