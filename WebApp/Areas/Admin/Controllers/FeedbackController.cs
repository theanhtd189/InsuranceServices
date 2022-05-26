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
    public class FeedbackController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();

        // GET: Admin/Feedback
        public async Task<IActionResult> Index()
        {
            var insuranceOnlineContext = db.Feedbacks.Include(f => f.Customer).Include(f => f.Insurance);
            return View(await insuranceOnlineContext.ToListAsync());
        }

        // GET: Admin/Feedback/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await db.Feedbacks
                .Include(f => f.Customer)
                .Include(f => f.Insurance)
                .FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // GET: Admin/Feedback/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Id");
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id");
            return View();
        }

        // POST: Admin/Feedback/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedbackId,FeedbackContent,CreatedAt,UpdatedAt,InsuranceId,CustomerId")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Add(feedback);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Id", feedback.CustomerId);
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id", feedback.InsuranceId);
            return View(feedback);
        }

        // GET: Admin/Feedback/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await db.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Id", feedback.CustomerId);
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id", feedback.InsuranceId);
            return View(feedback);
        }

        // POST: Admin/Feedback/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeedbackId,FeedbackContent,CreatedAt,UpdatedAt,InsuranceId,CustomerId")] Feedback feedback)
        {
            if (id != feedback.FeedbackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(feedback);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackExists(feedback.FeedbackId))
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
            ViewData["CustomerId"] = new SelectList(db.Customers, "Id", "Id", feedback.CustomerId);
            ViewData["InsuranceId"] = new SelectList(db.Insurances, "Id", "Id", feedback.InsuranceId);
            return View(feedback);
        }

        // GET: Admin/Feedback/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Feedbacks == null)
            {
                return NotFound();
            }

            var feedback = await db.Feedbacks
                .Include(f => f.Customer)
                .Include(f => f.Insurance)
                .FirstOrDefaultAsync(m => m.FeedbackId == id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Admin/Feedback/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Feedbacks == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Feedbacks'  is null.");
            }
            var feedback = await db.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                db.Feedbacks.Remove(feedback);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackExists(int id)
        {
          return (db.Feedbacks?.Any(e => e.FeedbackId == id)).GetValueOrDefault();
        }
    }
}
