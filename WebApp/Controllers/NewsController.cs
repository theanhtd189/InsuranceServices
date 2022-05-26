using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();

        // GET: News
        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            var list = db.News.OrderBy(x=>x.Id);
            var List = list.ToPagedList(pageNumber, 6);
            ViewBag.Page = pageNumber;
            ViewBag.TotalPage = Math.Ceiling((double)List.Count / 6);
            return View(List);
        }

        // GET: News/Details/5
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || db.News == null)
            {
                return NotFound();
            }

            var news = await db.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,CreatedAt,Content")] News news)
        {
            if (ModelState.IsValid)
            {
                db.Add(news);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.News == null)
            {
                return NotFound();
            }

            var news = await db.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,CreatedAt,Content")] News news)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(news);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
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
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.News == null)
            {
                return NotFound();
            }

            var news = await db.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.News == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.News'  is null.");
            }
            var news = await db.News.FindAsync(id);
            if (news != null)
            {
                db.News.Remove(news);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
          return (db.News?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
