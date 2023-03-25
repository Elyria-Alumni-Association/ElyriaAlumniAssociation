using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElyriaAlumniAssociation.Data;
using ElyriaAlumniAssociation.Models;
using Microsoft.AspNetCore.Identity;
using System.Net.NetworkInformation;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;

namespace ElyriaAlumniAssociation.Controllers
{
    public class AlumniController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AlumniController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Alumni
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> Index()
        {

            if (_context.Alumnus != null)
            {
                return View(await _context.Alumnus.ToListAsync());
            }

            return Problem("Entity set 'ApplicationDbContext.Alumnus'  is null.");
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> Index(string FirstNameSearch, string LastNameSearch, string LastNameAtGraduationSearch, string EmailSearch,
            string PhoneNumberSearch, string SchoolSearch, int GraduationYearStartSearch, int GraduationYearEndSearch)
        {
         
            var result = await _context.Alumnus.ToListAsync();

            if (!String.IsNullOrEmpty(FirstNameSearch))
            {
                FirstNameSearch = FirstNameSearch.ToUpper();
                result= result.Where(x => x.FirstName.ToUpper().Contains(FirstNameSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(LastNameSearch))
            {
                LastNameSearch = LastNameSearch.ToUpper();
                result = result.Where(x => x.LastName.ToUpper().Contains(LastNameSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(LastNameAtGraduationSearch))
            {
                LastNameAtGraduationSearch = LastNameAtGraduationSearch.ToUpper();
                result = result.Where(x => x.LastNameAtGraduation.ToUpper().Contains(LastNameAtGraduationSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(EmailSearch))
            {
                EmailSearch = EmailSearch.ToUpper();
                result = result.Where(x => x.EmailAddress.ToUpper().Contains(EmailSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(PhoneNumberSearch))
            {
                PhoneNumberSearch = PhoneNumberSearch.ToUpper();
                result = result.Where(x => x.PhoneNumber.ToUpper().Contains(PhoneNumberSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(SchoolSearch))
            {
                SchoolSearch = SchoolSearch.ToUpper();
                result = result.Where(x => x.School.ToUpper().Contains(SchoolSearch)).ToList();
            }
            if (GraduationYearStartSearch != 0 || GraduationYearEndSearch != 0)
            {
                if(GraduationYearStartSearch == 0)
                {
                    GraduationYearStartSearch = 1900;
                }

                if (GraduationYearEndSearch == 0)
                {
                    string year = DateTime.Now.Year.ToString();

                    GraduationYearEndSearch = Convert.ToInt32(year, 10);
                }

                result = result.Where(x => x.GraduationYear >= GraduationYearStartSearch &&  x.GraduationYear <= GraduationYearEndSearch).ToList();
            }

            return View(result);
        }

        // GET: Alumni/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Alumnus == null)
            {
                return NotFound();
            }

            var alumnus = await _context.Alumnus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alumnus == null)
            {
                return NotFound();
            }

            return View(alumnus);
        }

        // GET: Alumni/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alumni/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleInitial,LastNameAtGraduation,School,GraduationYear,StreetAddress,City,Country,PostalCode,EmailAddress,PhoneNumber,ScholasticAward,Athletics,Theatre,Band,Choir,Clubs,ClassOfficer,ROTC,OtherActivities,CurrentStatus")] Alumnus alumnus)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (ModelState.IsValid)
            {
                _context.Add(alumnus);
                await _context.SaveChangesAsync();
                if (user == null)
                {
                    return Redirect("ThankYou");
                } else
                {
                    return Redirect("Index");
                }
                   
            }
            return View(alumnus);
        }

        // GET: Alumni/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Alumnus == null)
            {
                return NotFound();
            }

            var alumnus = await _context.Alumnus.FindAsync(id);
            if (alumnus == null)
            {
                return NotFound();
            }
            return View(alumnus);
        }

        // POST: Alumni/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleInitial,LastNameAtGraduation,School,GraduationYear,StreetAddress,City,Country,PostalCode,EmailAddress,PhoneNumber,ScholasticAward,Athletics,Theatre,Band,Choir,Clubs,ClassOfficer,ROTC,OtherActivities,CurrentStatus")] Alumnus alumnus)
        {

            if (id != alumnus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumnus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumnusExists(alumnus.Id))
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
            return View(alumnus);
        }

        // GET: Alumni/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Alumnus == null)
            {
                return NotFound();
            }

            var alumnus = await _context.Alumnus
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alumnus == null)
            {
                return NotFound();
            }

            return View(alumnus);
        }

        // POST: Alumni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Alumnus == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Alumnus'  is null.");
            }
            var alumnus = await _context.Alumnus.FindAsync(id);
            if (alumnus != null)
            {
                _context.Alumnus.Remove(alumnus);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAlumni(List<Alumnus>? alumni)
        {
            List<Alumnus> alumniToDelete = new List<Alumnus>();
            foreach (var alumnus in alumni)
            {
                if (alumnus.Selected)
                {
                    alumniToDelete.Add(alumnus);
                }
            }

            _context.Alumnus.RemoveRange(alumniToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        private bool AlumnusExists(int id)
        {
          return (_context.Alumnus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

}
