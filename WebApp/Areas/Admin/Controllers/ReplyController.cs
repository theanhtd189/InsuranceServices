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
    public class ReplyController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();


        // GET: Admin/Reply
        public async Task<IActionResult> Index(int page=1, int limit=10)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)db.Replies.ToList().Count / limit);
            if (limit != 10)
            {
                ViewBag.Limit = limit;
            }
            var insuranceOnlineContext = db.Replies.Include(r => r.Faqs);
            var result = await insuranceOnlineContext.ToPagedListAsync(page,limit);
            return View(result);
        }

        // GET: Admin/Reply/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Replies == null)
            {
                return NotFound();
            }

            var reply = await db.Replies
                .Include(r => r.Faqs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reply == null)
            {
                return NotFound();
            }

            return View(reply);
        }

        // GET: Admin/Reply/Create
        public IActionResult Create()
        {
            ViewData["FaqsId"] = new SelectList(db.Faqs, "Id", "Question");
            return View();
        }

        // POST: Admin/Reply/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Answer,CreatedAt,UpdatedAt,FaqsId")] Reply reply)
        {
            if (ModelState.IsValid)
            {
                db.Add(reply);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FaqsId"] = new SelectList(db.Faqs, "Id", "Question", reply.FaqsId);
            return View(reply);
        }

        // GET: Admin/Reply/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Replies == null)
            {
                return NotFound();
            }

            var reply = await db.Replies.FindAsync(id);
            if (reply == null)
            {
                return NotFound();
            }
            ViewData["FaqsId"] = new SelectList(db.Faqs, "Id", "Question", reply.FaqsId);
            return View(reply);
        }

        // POST: Admin/Reply/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Answer,CreatedAt,UpdatedAt,FaqsId")] Reply reply)
        {
            if (id != reply.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(reply);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReplyExists(reply.Id))
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
            ViewData["FaqsId"] = new SelectList(db.Faqs, "Id", "Question", reply.FaqsId);
            return View(reply);
        }

        // GET: Admin/Reply/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Replies == null)
            {
                return NotFound();
            }

            var reply = await db.Replies
                .Include(r => r.Faqs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reply == null)
            {
                return NotFound();
            }

            return View(reply);
        }

        // POST: Admin/Reply/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Replies == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Replies'  is null.");
            }
            var reply = await db.Replies.FindAsync(id);
            if (reply != null)
            {
                db.Replies.Remove(reply);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReplyExists(int id)
        {
          return (db.Replies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
