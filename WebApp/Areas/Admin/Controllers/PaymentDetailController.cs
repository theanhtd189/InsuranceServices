using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaymentDetailController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();

        // GET: Admin/PaymentDetail
        public async Task<IActionResult> Index()
        {
            var insuranceOnlineContext = db.PaymentDetails.Include(p => p.Payment).Include(p => p.Periodic);
            return View(await insuranceOnlineContext.ToListAsync());
        }

        // GET: Admin/PaymentDetail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.PaymentDetails == null)
            {
                return NotFound();
            }

            var paymentDetail = await db.PaymentDetails
                .Include(p => p.Payment)
                .Include(p => p.Periodic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentDetail == null)
            {
                return NotFound();
            }

            return View(paymentDetail);
        }

        // GET: Admin/PaymentDetail/Create
        public IActionResult Create(int? PaymentId, int? ContractId)
        {
            ViewData["PeriodicId"] = new SelectList(db.PeriodicPaymentMethods, "Id", "Name");
            if (PaymentId != null)
            {
                ViewBag.PaymentId = PaymentId;
            }
            if (ContractId != null)
            {
                ViewBag.ContractId = ContractId;
                var p = db.Payments.FirstOrDefault(x=>x.ContractId==ContractId);
                ViewBag.PaymentId = p.Id;
            }
            return View();
        }

        // POST: Admin/PaymentDetail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentDetail paymentDetail,int? ContractId)
        {
                db.Add(paymentDetail);
                await db.SaveChangesAsync();
                return RedirectToAction("Details","Contract", new {id= ContractId });
        }

        // GET: Admin/PaymentDetail/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || db.PaymentDetails == null)
            {
                return NotFound();
            }

            var paymentDetail = db.PaymentDetails.Include(x => x.Payment).FirstOrDefault(x=>x.Id==id);
            if (paymentDetail == null)
            {
                return NotFound();
            }
            ViewBag.PaymentId = paymentDetail.Payment.Id;
            ViewBag.ContractId = paymentDetail.Payment.ContractId;
            ViewData["PeriodicId"] = new SelectList(db.PeriodicPaymentMethods, "Id", "Name", paymentDetail.PeriodicId);
            return View(paymentDetail);
        }

        // POST: Admin/PaymentDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,PaymentDetail paymentDetail,int? ContractId)
        {
            if (id != paymentDetail.Id)
            {
                return NotFound();
            }

                try
                {
                    db.Update(paymentDetail);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentDetailExists(paymentDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Contract", new { id = ContractId });
        }

        // GET: Admin/PaymentDetail/Delete/5
        public async Task<IActionResult> Delete(int? id, int? contractid)
        {
            if (id == null || db.PaymentDetails == null)
            {
                return NotFound();
            }

            var paymentDetail = await db.PaymentDetails
                .Include(p => p.Payment)
                .Include(p => p.Periodic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentDetail == null)
            {
                return NotFound();
            }
            if (contractid != null)
            {
                TempData["contractid"] = contractid;
            }
            return View(paymentDetail);
        }

        // POST: Admin/PaymentDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.PaymentDetails == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.PaymentDetails'  is null.");
            }
            var paymentDetail = await db.PaymentDetails.FindAsync(id);
            if (paymentDetail != null)
            {
                db.PaymentDetails.Remove(paymentDetail);
            }
            
            await db.SaveChangesAsync();
            if (TempData["contractid"] != null)
            {
                return RedirectToAction("Details","Contract",new {id=(int)TempData["contractid"] });
            }
            else
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentDetailExists(int id)
        {
          return (db.PaymentDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
