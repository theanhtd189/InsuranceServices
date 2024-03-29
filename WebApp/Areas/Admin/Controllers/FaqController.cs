﻿using System;
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
    public class FaqController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();
        // GET: Admin/Faq
        public async Task<IActionResult> Index(int page=1,int limit=10)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)db.Faqs.ToList().Count / limit);
            if (limit != 10)
            {
                ViewBag.Limit = limit;
            }
            var result = await db.Faqs.OrderBy(x => x.Id).Include(x=>x.Replies).ToPagedListAsync(page,limit);
              return db.Faqs != null ? 
                          View(result) :
                          Problem("Entity set 'InsuranceOnlineContext.Faqs'  is null.");
        }

        // GET: Admin/Faq/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Faqs == null)
            {
                return NotFound();
            }

            var faq = await db.Faqs.Include(x=>x.Replies)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }


        // GET: Admin/Faq/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Faq/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,CreatedAt,UpdatedAt")] Faq faq)
        {
            if (ModelState.IsValid)
            {
                db.Add(faq);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faq);
        }

        // GET: Admin/Faq/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Faqs == null)
            {
                return NotFound();
            }

            var faq = await db.Faqs.FindAsync(id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        // POST: Admin/Faq/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,CreatedAt,UpdatedAt")] Faq faq)
        {
            if (id != faq.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(faq);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaqExists(faq.Id))
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
            return View(faq);
        }

        // GET: Admin/Faq/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Faqs == null)
            {
                return NotFound();
            }

            var faq = await db.Faqs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (faq == null)
            {
                return NotFound();
            }

            return View(faq);
        }

        // POST: Admin/Faq/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Faqs == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Faqs'  is null.");
            }
            var faq = await db.Faqs.FindAsync(id);
            if (faq != null)
            {
                db.Faqs.Remove(faq);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FaqExists(int id)
        {
          return (db.Faqs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
