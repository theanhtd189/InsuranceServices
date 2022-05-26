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
    public class CustomerController : Controller
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();


        // GET: Admin/Customer
        public async Task<IActionResult> Index()
        {
              return db.Customers != null ? 
                          View(await db.Customers.ToListAsync()) :
                          Problem("Entity set 'InsuranceOnlineContext.Customers'  is null.");
        }

        [HttpGet]
        public async Task<ActionResult> Disable(int id)
        {
            if (db.Customers == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Customers'  is null.");
            }
            var customer = await db.Customers.FindAsync(id);
            if (customer != null)
            {
                customer.Status = false;
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Admin/Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || db.Customers == null)
            {
                return NotFound();
            }

            var customer = await db.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,Gender,Birthday,Phone,Address,Email,CreatedAt,UpdatedAt")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Add(customer);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Admin/Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Customers == null)
            {
                return NotFound();
            }

            var customer = await db.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Admin/Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Gender,Birthday,Phone,Address,Email,CreatedAt,UpdatedAt")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(customer);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Admin/Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Customers == null)
            {
                return NotFound();
            }

            var customer = await db.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Customers == null)
            {
                return Problem("Entity set 'InsuranceOnlineContext.Customers'  is null.");
            }
            var customer = await db.Customers.FindAsync(id);
            if (customer != null)
            {
                db.Customers.Remove(customer);
            }
            
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (db.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
