using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using X.PagedList;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();
        [Obsolete]
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;

        [Obsolete]
        public NewsController(IWebHostEnvironment environment, IConfiguration Aconfiguration)
        {
            hostingEnvironment = environment;
            configuration = Aconfiguration;
        }
        public async Task<IActionResult> Index(int page=1, int limit=10)
        {
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)db.News.ToList().Count / limit);
            if (limit != 10)
            {
                ViewBag.Limit = limit;
            }
            var result = await db.News.OrderBy(x=>x.Id).ToPagedListAsync(page,limit);
              return db.News != null ? 
                          View(result) :
                          Problem("Entity set 'InsuranceOnlineContext.News'  is null.");
        }

        // GET: Admin/News/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Admin/News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create(News news, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);

                    var sys_dir = hostingEnvironment.WebRootPath;
                    var path = Path.Combine(sys_dir + configuration["News_Image_Dir"],
                                        fileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await ImageFile.CopyToAsync(stream);
                        news.Image = configuration["News_Image_Dir"] + fileName;
                    }
                }
                
                db.Add(news);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: Admin/News/Edit/5
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

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(int id, News news, IFormFile? ImageFile)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImageFile != null)
                    {
                        var fileName = Path.GetFileName(ImageFile.FileName);

                        var sys_dir = hostingEnvironment.WebRootPath;
                        var path = Path.Combine(sys_dir + configuration["News_Image_Dir"],
                                            fileName);

                        using (var stream = System.IO.File.Create(path))
                        {
                            await ImageFile.CopyToAsync(stream);
                            news.Image = configuration["News_Image_Dir"] + fileName;
                        }
                    }
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

        // GET: Admin/News/Delete/5
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

        // POST: Admin/News/Delete/5
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
