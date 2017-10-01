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
    public class PatientsController : Controller
    {
        private readonly PatientContext _context;

        public PatientsController(PatientContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;            }


            ViewData["CurrentFilter"] = searchString;
            // Without stored procedure
            // var patients = from p in _context.Patients
            //               select p;

            // Using stored procedure:
            var patients = from p in _context.Patients.FromSql("usp_GetAllPatientsC")
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.LastName.Contains(searchString)
                    || p.FirstMidName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    patients = patients.OrderByDescending(p => p.LastName);
                    break;
                case "Date":
                    patients = patients.OrderBy(p => p.CreatedDate);
                    break;
                case "date_desc":
                    patients = patients.OrderByDescending(p => p.CreatedDate);
                    break;
                default:
                    patients = patients.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            // converts the student query to a single page of patients in a collection type that supports paging. 
            //  ?? represent the null - coalescing operator. The null - coalescing operator defines a default value for a nullable type
                return View(await PaginatedList<Patient>.CreateAsync(patients.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Patients/Details/5
        public async Task<IActionResult> Details(int? id, int? claimID)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                // Include navigation properties for Claims and Insurance Provider (within Claims)
                .Include (p => p.Claim)
                    .ThenInclude(c => c.InsProvider)
                // If no updating is needed in this context, improves performance.
                .AsNoTracking()
                // Retrieve a single patient entity.
                .SingleOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // SECURITY: Bind limits the properties that can be set to prevent hacks from adding their own.
        public async Task<IActionResult> Create([Bind("LastName,FirstMidName,CreatedDate")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(patient);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.SingleOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Patients.SingleOrDefaultAsync(s => s.ID == id);
            // SECURITY: Use TryUpdateModel instead of BIND. This will use data supplied by user...
            // ... When saveChanges(), EF creates SQL statements to update DB row. At that time...
            // ... only fields updated by user are updated in DB.
            if (await TryUpdateModelAsync<Patient>(
                studentToUpdate,
                "", // Prefix to form field names goes here.
                    // Below fields are whitelisted -- Only these allowed for use to preven overposting attacks.
                p => p.FirstMidName, p => p.LastName, p => p.CreatedDate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Patients/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (patient == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Please try again. If the problem persists " +
                    "see you system administrator.";
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var patient = await _context.Patients
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (patient == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateException /* ex */)
            {
                // Log the error (uncomment ex variable name and write log)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }


        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }
    }
}
