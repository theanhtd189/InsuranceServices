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
    public class PolicyController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();

        // GET: Admin/Policy
        public async Task<IActionResult> Index()
        {
            var insuranceOnlineContext = db.Policies.Include(p => p.Insurance);
            return View(await insuranceOnlineContext.ToListAsync());
        }

        // GET: Admin/Policy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Policies == null)
            {
                return NotFound();
            }

            var policy = await db.Policies
                .Include(p => p.Insurance)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // GET: Admin/Policy/Create
        public IActionResult Create()
        {
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id");
            return View();
        }

        // POST: Admin/Policy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreatedAt,UpdatedAt,InsuranceId")] Policy policy)
        {
            if (ModelState.IsValid)
            {
                db.Add(policy);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id", policy.InsuranceId);
            return View(policy);
        }

        // GET: Admin/Policy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Policies == null)
            {
                return NotFound();
            }

            var policy = await db.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id", policy.InsuranceId);
            return View(policy);
        }

        // POST: Admin/Policy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedAt,UpdatedAt,InsuranceId")] Policy policy)
        {
            if (id != policy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(policy);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyExists(policy.Id))
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
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id", policy.InsuranceId);
            return View(policy);
        }

        // GET: Admin/Policy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Policies == null)
            {
                return NotFound();
            }

            var policy = await db.Policies
                .Include(p => p.Insurance)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // POST: Admin/Policy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Policies == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Policies'  is null.");
            }
            var policy = await db.Policies.FindAsync(id);
            if (policy != null)
            {
                db.Policies.Remove(policy);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PolicyExists(int id)
        {
          return (db.Policies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
