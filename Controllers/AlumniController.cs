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
using ElyriaAlumniAssociation.Utils;

namespace ElyriaAlumniAssociation.Controllers
{
    public class AlumniController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DeletedAlumnusDbContext _deletedContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AlumniController> _logger;
        private readonly IEmailSender _emailSender;

        public AlumniController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore,
            DeletedAlumnusDbContext deletedContext,
            ILogger<AlumniController> logger,
            IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _deletedContext = deletedContext;
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
        public async Task<IActionResult> Index(string sortOrder, string firstNameSearch, string lastNameSearch, string schoolSearch, string graduationYearStartSearch, 
            string graduationYearEndSearch,string citySearch, string stateSearch, string countrySearch, bool scholasticSearch, bool athleticsSearch, bool theatreSearch, bool bandSearch, 
            bool choirSearch, bool clubsSearch,bool classOfficerSearch, bool rotcSearch, string otherSearch, string currentFirstNameFilter, string currentLastNameFilter, string currentSchoolFilter, 
            string currentGraduationYearStartFilter, string currentGraduationYearEndFilter,string currentCityFilter, string currentStateFilter, string currentCountryFilter, 
            bool currentScholasticFilter, bool currentAthleticsFilter, bool currentTheatreFilter, bool currentBandFilter, bool currentChoirFilter, bool currentClubsFilter,
            bool currentClassOfficerFilter, bool currentRotcFilter, string currentOtherFilter, int? pageNumber)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["LastNameSortParam"] = sortOrder == "last_name" ? "last_name_desc" : "last_name";
            ViewData["FirstNameSortParam"] = sortOrder == "first_name" ? "first_name_desc" : "first_name";
            ViewData["SchoolSortParam"] = sortOrder == "school" ? "school_desc" : "school";
            ViewData["GraduationSortParam"] = sortOrder == "graduation" ? "graduation_desc" : "graduation";
            ViewData["CitySortParam"] = sortOrder == "city" ? "city_desc" : "city";
            ViewData["StateSortParam"] = sortOrder == "state" ? "state_desc" : "state";
            ViewData["CountrySortParam"] = sortOrder == "country" ? "country_desc" : "country";

            if (otherSearch != null || firstNameSearch != null || lastNameSearch != null || schoolSearch != null || graduationYearStartSearch != null || graduationYearEndSearch != null
                || citySearch != null || stateSearch != null || countrySearch != null || scholasticSearch || athleticsSearch || theatreSearch || bandSearch || choirSearch
                || clubsSearch || classOfficerSearch || rotcSearch)
            {
                pageNumber = 1;
            }
            else
            {
                firstNameSearch = currentFirstNameFilter;
                lastNameSearch = currentLastNameFilter;
                schoolSearch = currentSchoolFilter;
                graduationYearStartSearch = currentGraduationYearStartFilter;
                graduationYearEndSearch = currentGraduationYearEndFilter;
                citySearch = currentCityFilter;
                stateSearch = currentStateFilter;
                countrySearch = currentCountryFilter;
                scholasticSearch = currentScholasticFilter;
                athleticsSearch = currentAthleticsFilter;
                theatreSearch = currentTheatreFilter;
                bandSearch = currentBandFilter;
                choirSearch = currentChoirFilter;
                clubsSearch = currentClubsFilter;
                classOfficerSearch = currentClassOfficerFilter;
                rotcSearch = currentRotcFilter;
                otherSearch = currentOtherFilter;
            }

            ViewData["FirstNameFilter"] = firstNameSearch;
            ViewData["LastNameFilter"] = lastNameSearch;
            ViewData["SchoolFilter"] = schoolSearch;
            ViewData["GraduationStartFilter"] = graduationYearStartSearch;
            ViewData["GraduationEndFilter"] = graduationYearEndSearch;
            ViewData["CityFilter"] = citySearch;
            ViewData["StateFilter"] = stateSearch;
            ViewData["CountryFilter"] = countrySearch;
            ViewData["ScholasticFilter"] = scholasticSearch;
            ViewData["AthleticsFilter"] = athleticsSearch;
            ViewData["TheatreFilter"] = theatreSearch;
            ViewData["BandFilter"] = bandSearch;
            ViewData["ChoirFilter"] = choirSearch;
            ViewData["ClubsFilter"] = clubsSearch;
            ViewData["ClassOfficerFilter"] = classOfficerSearch;
            ViewData["ROTCFilter"] = rotcSearch;
            ViewData["OtherFilter"] = otherSearch;

            var results = await _context.Alumnus.ToListAsync();

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

            results = filter(results, firstNameSearch, lastNameSearch, schoolSearch, graduationYearStartSearch, graduationYearEndSearch,
            citySearch, stateSearch, countrySearch, scholasticSearch, athleticsSearch, theatreSearch, bandSearch, choirSearch, clubsSearch,
            classOfficerSearch, rotcSearch, otherSearch);

            int pageSize = 5;
            return View(PaginatedList<Alumnus>.Create(results, pageNumber ?? 1, pageSize));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveSelected(List<Alumnus>? alumni, int? pageNumber)
        {

            foreach (Alumnus alumnus in alumni)
            {
                var foundAlumnus = await _context.Alumnus.FirstOrDefaultAsync(x => x.Id == alumnus.Id);
                foundAlumnus.Selected = alumnus.Selected;
                _context.Alumnus.Update(_context.Alumnus.Find(foundAlumnus.Id));
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new {pageNumber = pageNumber});
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
            var deletedAlumnus = ConvertAlumnusToDeletedAlumnus(alumnus);

            if(deletedAlumnus != null)
            {
                _deletedContext.DeletedAlumnus.Add(deletedAlumnus);
            }

            if (alumnus != null)
            {
                _context.Alumnus.Remove(alumnus);
            }

            await _deletedContext.SaveChangesAsync();
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteAlumni()
        {
            List<Alumnus> alumni = await _context.Alumnus.ToListAsync();
            List<Alumnus> alumniToDelete = new List<Alumnus>();
            foreach (var alumnus in alumni)
            {
                if (alumnus.Selected)
                {
                    var foundAlumnus = await _context.Alumnus.FirstOrDefaultAsync(x => x.Id == alumnus.Id);
                    var deletedAlumnus = ConvertAlumnusToDeletedAlumnus(foundAlumnus);
                    _deletedContext.DeletedAlumnus.Add(deletedAlumnus);
                    _context.Alumnus.Remove(_context.Alumnus.Find(alumnus.Id));
                }
            }

            await _deletedContext.SaveChangesAsync();
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Export()
        {
            List<Alumnus> alumni = await _context.Alumnus.ToListAsync();
            List<Alumnus> alumniToExport = new List<Alumnus>();
            foreach (var alumnus in alumni)
            {
                if (alumnus.Selected)
                {
                    var foundAlumnus = await _context.Alumnus.FirstOrDefaultAsync(x => x.Id == alumnus.Id);
                 
                    alumniToExport.Add(foundAlumnus);
                   
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

        private List<Alumnus> filter(List<Alumnus> results, string firstNameSearch, string lastNameSearch, string schoolSearch, string graduationYearStartSearch, string graduationYearEndSearch,
            string citySearch, string stateSearch, string countrySearch, bool scholasticSearch, bool athleticsSearch, bool theatreSearch, bool bandSearch, bool choirSearch, bool clubsSearch,
            bool classOfficerSearch, bool rotcSearch, string otherSearch)
        {
            if (!String.IsNullOrEmpty(firstNameSearch))
            {
                firstNameSearch = firstNameSearch.ToUpper();
                results = results.Where(x => x.FirstName.ToUpper().Contains(firstNameSearch)).ToList();
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
                    try
                    {
                        startYear = int.Parse(graduationYearStartSearch);
                    }
                    catch
                    {
                        startYear = 1900;
                    }
                }

                if (!String.IsNullOrEmpty(graduationYearEndSearch))
                {
                    try
                    {
                        endYear = int.Parse(graduationYearEndSearch);
                    }
                    catch
                    {
                        endYear = Convert.ToInt32(year, 10);
                    }
                }


                results = results.Where(x => x.GraduationYear >= startYear && x.GraduationYear <= endYear).ToList();
            }
            if (!String.IsNullOrEmpty(citySearch))
            {
                citySearch = citySearch.ToUpper();
                results = results.Where(x => x.City.ToUpper().Contains(citySearch)).ToList();
            }
            if (!String.IsNullOrEmpty(stateSearch))
            {
                stateSearch = stateSearch.ToUpper();
                results = results.Where(x => x.State.ToUpper().Contains(stateSearch)).ToList();
            }
            if (!String.IsNullOrEmpty(countrySearch))
            {
                countrySearch = countrySearch.ToUpper();
                results = results.Where(x => x.Country.ToUpper().Contains(countrySearch)).ToList();
            }
            if (scholasticSearch)
            {
                results = results.Where(x => x.ScholasticAward == scholasticSearch).ToList();
            }
            if (athleticsSearch)
            {
                results = results.Where(x => x.Athletics == athleticsSearch).ToList();
            }
            if (theatreSearch)
            {
                results = results.Where(x => x.Theatre == theatreSearch).ToList();
            }
            if (bandSearch)
            {
                results = results.Where(x => x.Band == bandSearch).ToList();
            }
            if (choirSearch)
            {
                results = results.Where(x => x.Choir == choirSearch).ToList();
            }
            if (clubsSearch)
            {
                results = results.Where(x => x.Clubs == clubsSearch).ToList();
            }
            if (classOfficerSearch)
            {
                results = results.Where(x => x.ClassOfficer == classOfficerSearch).ToList();
            }
            if (rotcSearch)
            {
                results = results.Where(x => x.ROTC == rotcSearch).ToList();
            }
            if (!String.IsNullOrEmpty(otherSearch))
            {
                otherSearch = otherSearch.ToUpper();
                results = results.Where(x => x.OtherActivities == otherSearch).ToList();
            }

            return results;
        }

        private DeletedAlumnus ConvertAlumnusToDeletedAlumnus(Alumnus alumnus)
        {
            DeletedAlumnus deletedAlumnus = new DeletedAlumnus();
            deletedAlumnus.FirstName = alumnus.FirstName;
            deletedAlumnus.LastName = alumnus.LastName;
            deletedAlumnus.MiddleInitial = alumnus.MiddleInitial;
            deletedAlumnus.LastNameAtGraduation = alumnus.LastNameAtGraduation;
            deletedAlumnus.School = alumnus.School;
            deletedAlumnus.GraduationYear = alumnus.GraduationYear;
            deletedAlumnus.StreetAddress = alumnus.StreetAddress;
            deletedAlumnus.City = alumnus.City;
            deletedAlumnus.State = alumnus.State;
            deletedAlumnus.Country = alumnus.Country;
            deletedAlumnus.PostalCode = alumnus.PostalCode;
            deletedAlumnus.EmailAddress = alumnus.EmailAddress;
            deletedAlumnus.PhoneNumber = alumnus.PhoneNumber;
            deletedAlumnus.ScholasticAward = alumnus.ScholasticAward;
            deletedAlumnus.Athletics = alumnus.Athletics;
            deletedAlumnus.Theatre = alumnus.Theatre;
            deletedAlumnus.Band = alumnus.Band;
            deletedAlumnus.Choir = alumnus.Choir;
            deletedAlumnus.Clubs = alumnus.Clubs;
            deletedAlumnus.ClassOfficer = alumnus.ClassOfficer;
            deletedAlumnus.ROTC = alumnus.ROTC;
            deletedAlumnus.OtherActivities = alumnus.OtherActivities;
            deletedAlumnus.CurrentStatus = alumnus.CurrentStatus;
            deletedAlumnus.Selected = false;

            return deletedAlumnus;
        }

        private Alumnus DuplicateAlumnus(Alumnus alumnus)
        {
            Alumnus duplicateAlumnus = new Alumnus();
            duplicateAlumnus.FirstName = alumnus.FirstName;
            duplicateAlumnus.LastName = alumnus.LastName;
            duplicateAlumnus.MiddleInitial = alumnus.MiddleInitial;
            duplicateAlumnus.LastNameAtGraduation = alumnus.LastNameAtGraduation;
            duplicateAlumnus.School = alumnus.School;
            duplicateAlumnus.GraduationYear = alumnus.GraduationYear;
            duplicateAlumnus.StreetAddress = alumnus.StreetAddress;
            duplicateAlumnus.City = alumnus.City;
            duplicateAlumnus.State = alumnus.State;
            duplicateAlumnus.Country = alumnus.Country;
            duplicateAlumnus.PostalCode = alumnus.PostalCode;
            duplicateAlumnus.EmailAddress = alumnus.EmailAddress;
            duplicateAlumnus.PhoneNumber = alumnus.PhoneNumber;
            duplicateAlumnus.ScholasticAward = alumnus.ScholasticAward;
            duplicateAlumnus.Athletics = alumnus.Athletics;
            duplicateAlumnus.Theatre = alumnus.Theatre;
            duplicateAlumnus.Band = alumnus.Band;
            duplicateAlumnus.Choir = alumnus.Choir;
            duplicateAlumnus.Clubs = alumnus.Clubs;
            duplicateAlumnus.ClassOfficer = alumnus.ClassOfficer;
            duplicateAlumnus.ROTC = alumnus.ROTC;
            duplicateAlumnus.OtherActivities = alumnus.OtherActivities;
            duplicateAlumnus.CurrentStatus = alumnus.CurrentStatus;
            duplicateAlumnus.Selected = false;

            return duplicateAlumnus;
        }
    }

}
