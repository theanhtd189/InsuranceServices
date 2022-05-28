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
    public class ContractController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();


        // GET: Admin/Contract
        public async Task<IActionResult> Index(int page=1, int limit =10)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)db.Contracts.ToList().Count / limit);
            if (limit != 10)
            {
                ViewBag.Limit = limit;
            }
            var insuranceOnlineContext = db.Contracts.Include(c => c.Customer).Include(c => c.Insurance);
            var result = await insuranceOnlineContext.ToPagedListAsync(page, limit);
            return db.Contracts != null ?
                          View(result) :
                          Problem("Entity set 'InsuranceOnlineContext.Contracts'  is null.");
        }

        // GET: Admin/Contract/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Contracts == null)
            {
                return NotFound();
            }

            var contract = await db.Contracts
                .Include(c => c.Customer)
                .Include(c => c.Insurance)
                .Include(x=>x.Payments).ThenInclude(x=>x.PaymentDetails)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        // GET: Admin/Contract/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Username");
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Name");
            return View();
        }

        // POST: Admin/Contract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Beneficiary,Duration,CreatedAt,ExpiredAt,CustomerId,InsuranceId,Status,Total")] Contract contract)
        {

                db.Add(contract);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
/*      
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Username", contract.CustomerId);
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Name", contract.InsuranceId);
            return View(contract);*/
        }

        // GET: Admin/Contract/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Contracts == null)
            {
                return NotFound();
            }

            var contract = await db.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Username", contract.CustomerId);
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Name", contract.InsuranceId);
            return View(contract);
        }

        // POST: Admin/Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Beneficiary,Duration,CreatedAt,ExpiredAt,CustomerId,InsuranceId,Status,Total")] Contract contract)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(contract);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractExists(contract.Id))
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
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Username", contract.CustomerId);
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Name", contract.InsuranceId);
            return View(contract);
        }

        // GET: Admin/Contract/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Contracts == null)
            {
                return NotFound();
            }

            var contract = await db.Contracts
                .Include(c => c.Customer)
                .Include(c => c.Insurance)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Admin/Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Contracts == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Contracts'  is null.");
            }
            var contract = await db.Contracts.FindAsync(id);
            if (contract != null)
            {
                db.Contracts.Remove(contract);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractExists(int id)
        {
          return (db.Contracts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
