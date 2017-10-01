using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthcareNetCoreSample.Data;
using HealthcareNetCoreSample.Models;

namespace HealthcareNetCoreSample.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly PatientContext _context;

        public ClaimsController(PatientContext context)
        {
            _context = context;
        }

        // GET: Claims
        public async Task<IActionResult> Index()
        {
            var claims = _context.Claims
                .Include(c => c.InsProvider)
                .Include(c => c.Patient)
                .AsNoTracking();
            return View(await claims.ToListAsync());
        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims
                .Include(c => c.InsProvider)
                .Include(c => c.Patient)
                .SingleOrDefaultAsync(m => m.ClaimID == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // GET: Claims/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                ViewData["InsProviderID"] = new SelectList(_context.InsProviders, "InsProviderID", "InsProviderID");
                ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "FullName");
                return View();
            };

            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewData["InsProviderID"] = new SelectList(_context.InsProviders, "InsProviderID", "InsProviderID");
            ViewData["PatientID"] = id;
            //ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "ID");
            ViewData["PatientName"] = patient.FullName;


            return View();

        }

        // POST: Claims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaimID,PatientID,InsProviderID,AmountOwed,ClaimStatus")] Claim claim)
        {
            
            try
            {

                if (ModelState.IsValid)
                {
                    _context.Add(claim);
                    await _context.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Details", "Patients", new { id = claim.PatientID });
                }
            }

            //ViewData["InsProviderID"] = new SelectList(_context.InsProviders, "InsProviderID", "InsProviderID", claim.InsProviderID);
            //ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "FirstMidName", claim.PatientID);

            catch (DbUpdateException /* ex */)
            {
                Console.Write(claim);
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(claim);
            //return RedirectToAction("Details", "Patients", new { id = 3 });
        }

        // GET: Claims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims.SingleOrDefaultAsync(m => m.ClaimID == id);
            if (claim == null)
            {
                return NotFound();
            }
            ViewData["InsProviderID"] = new SelectList(_context.InsProviders, "InsProviderID", "InsProviderID", claim.InsProviderID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "FirstMidName", claim.PatientID);
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaimID,PatientID,InsProviderID,AmountOwed,ClaimStatus")] Claim claim)
        {
            if (id != claim.ClaimID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(claim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaimExists(claim.ClaimID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Patients", new { id = claim.PatientID });

            }
            ViewData["InsProviderID"] = new SelectList(_context.InsProviders, "InsProviderID", "InsProviderID", claim.InsProviderID);
            ViewData["PatientID"] = new SelectList(_context.Patients, "ID", "FirstMidName", claim.PatientID);
            return View(claim);
        }

        // GET: Claims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claims
                .Include(c => c.InsProvider)
                .Include(c => c.Patient)
                .SingleOrDefaultAsync(m => m.ClaimID == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var claim = await _context.Claims.SingleOrDefaultAsync(m => m.ClaimID == id);
            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Patients", new { id = claim.PatientID });

        }

        private bool ClaimExists(int id)
        {
            return _context.Claims.Any(e => e.ClaimID == id);
        }
    }
}
