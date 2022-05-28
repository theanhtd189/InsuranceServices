using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaymentController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();


        // GET: Admin/Payment
        public async Task<IActionResult> Index(int page=1, int limit=10)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)db.Payments.ToList().Count / limit);
            if (limit != 10)
            {
                ViewBag.Limit = limit;
            }
            var insuranceOnlineContext = db.Payments.Include(p => p.Contract);
            var result = await insuranceOnlineContext.OrderBy(x => x.Id).ToPagedListAsync(page,limit);
            return View(result);
        }

        // GET: Admin/Payment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Payments == null)
            {
                return NotFound();
            }

            var payment = await db.Payments
                .Include(p => p.Contract)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // GET: Admin/Payment/Create
        public IActionResult Create()
        {
            ViewData["ContractId"] = new SelectList(db.Contracts, "Id", "Id");
            return View();
        }

        // POST: Admin/Payment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,ContractId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Add(payment);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContractId"] = new SelectList(db.Contracts, "Id", "Id", payment.ContractId);
            return View(payment);
        }

        // GET: Admin/Payment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Payments == null)
            {
                return NotFound();
            }

            var payment = await db.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["ContractId"] = new SelectList(db.Contracts, "Id", "Id", payment.ContractId);
            return View(payment);
        }

        // POST: Admin/Payment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Total,ContractId")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(payment);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ContractId"] = new SelectList(db.Contracts, "Id", "Id", payment.ContractId);
            return View(payment);
        }

        // GET: Admin/Payment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Payments == null)
            {
                return NotFound();
            }

            var payment = await db.Payments
                .Include(p => p.Contract)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Admin/Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Payments == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Payments'  is null.");
            }
            var payment = await db.Payments.FindAsync(id);
            if (payment != null)
            {
                db.Payments.Remove(payment);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
          return (db.Payments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
