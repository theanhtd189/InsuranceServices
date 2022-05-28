using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using Microsoft.Extensions.Hosting.Internal;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using X.PagedList;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InsuranceController : Controller
    {
        private readonly InsuranceOnlineContext db = new InsuranceOnlineContext();
        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IConfiguration configuration;

        [Obsolete]
        public InsuranceController(IHostingEnvironment environment, IConfiguration Aconfiguration)
        {
            hostingEnvironment = environment;
            configuration = Aconfiguration;
        }
        // GET: Admin/Insurance
        public async Task<IActionResult> Index(int page=1,int limit=10)
        {
            var insuranceOnlineContext = db.Insurances.Include(i => i.Category);
            var result = await insuranceOnlineContext.ToPagedListAsync(page,limit);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPage = Math.Ceiling((decimal)insuranceOnlineContext.ToList().Count / limit);
            if (limit != 10)
            {
                ViewBag.Limit = limit;
            }
            return View(result);
        }

        // GET: Admin/Insurance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await db.Insurances
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // GET: Admin/Insurance/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Admin/Insurance/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Create(Insurance insurance, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile!=null)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);

                    var sys_dir = hostingEnvironment.WebRootPath;
                    var path = Path.Combine(sys_dir + configuration["Insurance_Image_Dir"],
                                        fileName);
 
                    using (var stream = System.IO.File.Create(path))
                    {
                        await ImageFile.CopyToAsync(stream);
                        insurance.Image = configuration["Insurance_Image_Dir"]+fileName;
                    }
                }

                db.Add(insurance);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Categories, "CategoryId", "CategoryName", insurance.CategoryId);
            return View(insurance);
        }

        // GET: Admin/Insurance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await db.Insurances.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(db.Categories, "CategoryId", "CategoryName", insurance.CategoryId);
            return View(insurance);
        }

        // POST: Admin/Insurance/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public async Task<IActionResult> Edit(int id, Insurance insurance, IFormFile? ImageFile)
        {
            if (id != insurance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    insurance.UpdatedAt = DateTime.Now;
                    if (ImageFile != null)
                    {
                        var fileName = Path.GetFileName(ImageFile.FileName);

                        var sys_dir = hostingEnvironment.WebRootPath;
                        var path = Path.Combine(sys_dir + configuration["Insurance_Image_Dir"],
                                            fileName);

                        using (var stream = System.IO.File.Create(path))
                        {
                            await ImageFile.CopyToAsync(stream);
                            insurance.Image = configuration["Insurance_Image_Dir"] + fileName;
                        }
                    }
                    db.Update(insurance);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceExists(insurance.Id))
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
            ViewData["CategoryId"] = new SelectList(db.Categories, "CategoryId", "CategoryName", insurance.CategoryId);
            return View(insurance);
        }

        // GET: Admin/Insurance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Insurances == null)
            {
                return NotFound();
            }

            var insurance = await db.Insurances
                .Include(i => i.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        // POST: Admin/Insurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Insurances == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Insurances'  is null.");
            }
            var insurance = await db.Insurances.FindAsync(id);
            if (insurance != null)
            {
                db.Insurances.Remove(insurance);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceExists(int id)
        {
          return (db.Insurances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
