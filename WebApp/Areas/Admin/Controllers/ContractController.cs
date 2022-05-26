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
    public class ContractController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();


        // GET: Admin/Contract
        public async Task<IActionResult> Index()
        {
            var insuranceOnlineContext = db.Contracts.Include(c => c.Customer).Include(c => c.Insurance);
            return View(await insuranceOnlineContext.ToListAsync());
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
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Id");
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id");
            return View();
        }

        // POST: Admin/Contract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Beneficiary,Duration,CreatedAt,ExpriredAt,CustomerId,InsuranceId,Status,Total")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Add(contract);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Id", contract.CustomerId);
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id", contract.InsuranceId);
            return View(contract);
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
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Id", contract.CustomerId);
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id", contract.InsuranceId);
            return View(contract);
        }

        // POST: Admin/Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Beneficiary,Duration,CreatedAt,ExpriredAt,CustomerId,InsuranceId,Status,Total")] Contract contract)
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
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Id", contract.CustomerId);
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id", contract.InsuranceId);
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
