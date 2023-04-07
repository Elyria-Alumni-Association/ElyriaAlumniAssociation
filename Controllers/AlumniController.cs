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
using Microsoft.VisualBasic;
using CsvHelper;
using CsvHelper.Configuration;
using ElyriaAlumniAssociation.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ElyriaAlumniAssociation.Controllers
{
    public class AlumniController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AlumniController> _logger;
        private readonly IEmailSender _emailSender;

        public AlumniController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<AlumniController> logger,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        // GET: Alumni
        [Authorize]
        public async Task<IActionResult> Index()
        {

            if (_context.Alumnus != null)
            {
                return View(await _context.Alumnus.ToListAsync());
            }

            return Problem("Entity set 'ApplicationDbContext.Alumnus'  is null.");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string sortOrder, string firstNameSearch, string lastNameSearch, string schoolSearch, string graduationYearStartSearch, string graduationYearEndSearch,
            string citySearch, string stateSearch, string countrySearch, bool scholasticSearch)
        {
            var results = await _context.Alumnus.ToListAsync();

            ViewData["LastNameSortParam"] = sortOrder == "last_name" ? "last_name_desc" : "last_name";
            ViewData["FirstNameSortParam"] = sortOrder == "first_name" ? "first_name_desc" : "first_name";
            ViewData["SchoolSortParam"] = sortOrder == "school" ? "school_desc" : "school";
            ViewData["GraduationSortParam"] = sortOrder == "graduation" ? "graduation_desc" : "graduation";
            ViewData["CitySortParam"] = sortOrder == "city" ? "city_desc" : "city";
            ViewData["StateSortParam"] = sortOrder == "state" ? "state_desc" : "state";
            ViewData["CountrySortParam"] = sortOrder == "country" ? "country_desc" : "country";

            ViewData["FirstNameFilter"] = firstNameSearch;
            ViewData["LastNameFilter"] = lastNameSearch;
            ViewData["SchoolFilter"] = schoolSearch;
            ViewData["GraduationStartFilter"] = graduationYearStartSearch;
            ViewData["GraduationEndFilter"] = graduationYearEndSearch;
            ViewData["CityFilter"] = firstNameSearch;
            ViewData["LastNameFilter"] = lastNameSearch;
            ViewData["SchoolFilter"] = schoolSearch;
            ViewData["ScholasticFilter"] = scholasticSearch;

            switch (sortOrder)
            {
                case "last_name_desc":
                    results = results.OrderByDescending(a => a.LastName).ToList();
                    break;
                case "last_name":
                    results = results.OrderBy(a => a.LastName).ToList();
                    break;
                case "first_name":
                    results = results.OrderBy(a => a.FirstName).ToList();
                    break;
                case "first_name_desc":
                    results = results.OrderByDescending(a => a.FirstName).ToList();
                    break;
                case "school":
                    results = results.OrderBy(a => a.School).ToList();
                    break;
                case "school_desc":
                    results = results.OrderByDescending(a => a.School).ToList();
                    break;
                case "graduation":
                    results = results.OrderBy(a => a.GraduationYear).ToList();
                    break;
                case "graduation_desc":
                    results = results.OrderByDescending(a => a.GraduationYear).ToList();
                    break;
                case "city":
                    results = results.OrderBy(a => a.City).ToList();
                    break;
                case "city_desc":
                    results = results.OrderByDescending(a => a.City).ToList();
                    break;
                case "state":
                    results = results.OrderBy(a => a.State).ToList();
                    break;
                case "state_desc":
                    results = results.OrderByDescending(a => a.State).ToList();
                    break;
                case "country":
                    results = results.OrderBy(a => a.Country).ToList();
                    break;
                case "country_desc":
                    results = results.OrderByDescending(a => a.Country).ToList();
                    break;
            }


            if (!String.IsNullOrEmpty(firstNameSearch))
            {
                firstNameSearch = firstNameSearch.ToUpper();
                results= results.Where(x => x.FirstName.ToUpper().Contains(firstNameSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(lastNameSearch))
            {
                lastNameSearch = lastNameSearch.ToUpper();
                results = results.Where(x => x.LastName.ToUpper().Contains(lastNameSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(schoolSearch))
            {
                schoolSearch = schoolSearch.ToUpper();
                results = results.Where(x => x.School.ToUpper().Contains(schoolSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(graduationYearStartSearch) || !String.IsNullOrEmpty(graduationYearEndSearch))
            {
                var startYear = 1900;
                string year = DateTime.Now.Year.ToString();
                var endYear = Convert.ToInt32(year, 10);

                if (!String.IsNullOrEmpty(graduationYearStartSearch))
                {
                    startYear = int.Parse(graduationYearStartSearch);
                }

                if (!String.IsNullOrEmpty(graduationYearEndSearch))
                {
                    endYear = int.Parse(graduationYearEndSearch);
                }


                results = results.Where(x => x.GraduationYear >= startYear &&  x.GraduationYear <= endYear).ToList();
            }
            if (scholasticSearch)
            {
                results = results.Where(x => x.ScholasticAward == scholasticSearch).ToList();
            }

            return View(results);
        }

        // GET: Alumni/Details/5
        [Authorize]
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
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleInitial,LastNameAtGraduation,School,GraduationYear,StreetAddress,City,Country,State,PostalCode,EmailAddress,PhoneNumber,ScholasticAward,Athletics,Theatre,Band,Choir,Clubs,ClassOfficer,ROTC,OtherActivities,CurrentStatus")] Alumnus alumnus)
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleInitial,LastNameAtGraduation,School,GraduationYear,StreetAddress,City,Country,State,PostalCode,EmailAddress,PhoneNumber,ScholasticAward,Athletics,Theatre,Band,Choir,Clubs,ClassOfficer,ROTC,OtherActivities,CurrentStatus")] Alumnus alumnus)
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
        [Authorize]
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
        [Authorize]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Export(List<Alumnus>? alumni)
        {
            List<Alumnus> alumniToExport = new List<Alumnus>();
            foreach (var alumnus in alumni)
            {
                if (alumnus.Selected)
                {
                    var foundAlumnus = await _context.Alumnus.FirstOrDefaultAsync(x => x.Id == alumnus.Id);
                    if(foundAlumnus != null)
                    {
                        alumniToExport.Add(foundAlumnus);
                    }
                }
            }

            string filePath = ".\\CSVFiles\\AlumniData.csv";
            string loggedInUser = User.Identity.Name;
            string message = "Attached is the data you requested at " + DateTime.Now.ToString() + ".";
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(alumniToExport);
            }

            await _emailSender.SendEmailAsync(loggedInUser, "Alumni Data", message);

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
