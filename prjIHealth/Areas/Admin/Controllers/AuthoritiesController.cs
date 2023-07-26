using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using prjIHealth.Models;
using prjIHealth.ViewModels;

namespace prjIHealth.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthoritiesController : Controller
    {
        private readonly IHealthContext _context;

        public AuthoritiesController(IHealthContext context)
        {
            _context = context;
        }

        // GET: Admin/Authorities
        public async Task<IActionResult> Index()
        {
            return View(await _context.TAuthorities.ToListAsync());
        }

        // GET: Admin/Authorities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tAuthority = await _context.TAuthorities
                .FirstOrDefaultAsync(m => m.FAutorityId == id);
            if (tAuthority == null)
            {
                return NotFound();
            }

            return View(tAuthority);
        }

        // GET: Admin/Authorities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Authorities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FAuthorityName,FRemarks")]TAuthority ta)
        {
            if (ModelState.IsValid)
            {
                _context.TAuthorities.Add(ta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ta);
        }

        // GET: Admin/Authorities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tAuthority = await _context.TAuthorities.FindAsync(id);
            if (tAuthority == null)
            {
                return NotFound();
            }
            return View(tAuthority);
        }

        // POST: Admin/Authorities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FAutorityId,FAuthorityName,FRemarks")] TAuthority tAuthority)
        {
            if (id != tAuthority.FAutorityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tAuthority);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TAuthorityExists(tAuthority.FAutorityId))
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
            return View(tAuthority);
        }

        // GET: Admin/Authorities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tAuthority = await _context.TAuthorities
                .FirstOrDefaultAsync(m => m.FAutorityId == id);
            if (tAuthority == null)
            {
                return NotFound();
            }

            return View(tAuthority);
        }

        // POST: Admin/Authorities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tAuthority = await _context.TAuthorities.FindAsync(id);
            _context.TAuthorities.Remove(tAuthority);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TAuthorityExists(int id)
        {
            return _context.TAuthorities.Any(e => e.FAutorityId == id);
        }
    }
}
