using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using X.PagedList;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();
        [Obsolete]
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;

        [Obsolete]
        public CategoryController(Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, IConfiguration Aconfiguration)
        {
            hostingEnvironment = environment;
            configuration = Aconfiguration;
        }
        // GET: Admin/Category
        public async Task<IActionResult> Index(int page = 1, int limit = 10)
        {
            var result = await db.Categories.Include(x => x.Insurances).OrderBy(x=>x.CategoryId).ToPagedListAsync(page, limit);
            ViewBag.TotalPage = Math.Ceiling((decimal)db.Categories.ToList().Count / limit);
            ViewBag.CurrentPage = page;
            if (limit != 10)
            {
                ViewBag.Limit = limit;
            }
            return db.Categories != null ?
                        View(result) :
                        Problem("Entity set 'InsuranceOnlineContext.Categories'  is null.");
        }

        // GET: Admin/Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Categories == null)
            {
                return NotFound();
            }

            var category = await db.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create(Category category, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);

                    var sys_dir = hostingEnvironment.WebRootPath;
                    var path = Path.Combine(sys_dir + configuration["InsuranceType_Image_Dir"],
                                        fileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await ImageFile.CopyToAsync(stream);
                        category.Image = configuration["InsuranceType_Image_Dir"] + fileName;
                    }
                }
                category.UpdatedAt = DateTime.Now;
                db.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Categories == null)
            {
                return NotFound();
            }

            var category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(int id, Category category, IFormFile? ImageFile)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }
            try
            {
                category.UpdatedAt = DateTime.Now;
                if (ImageFile != null)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);

                    var sys_dir = hostingEnvironment.WebRootPath;
                    var path = Path.Combine(sys_dir + configuration["InsuranceType_Image_Dir"],
                                        fileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await ImageFile.CopyToAsync(stream);
                        category.Image = configuration["InsuranceType_Image_Dir"] + fileName;
                    }
                }
                db.Update(category);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.CategoryId))
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

        // GET: Admin/Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Categories == null)
            {
                return NotFound();
            }

            var category = await db.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Categories == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Categories'  is null.");
            }
            var category = await db.Categories.FindAsync(id);
            if (category != null)
            {
                db.Categories.Remove(category);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return (db.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
