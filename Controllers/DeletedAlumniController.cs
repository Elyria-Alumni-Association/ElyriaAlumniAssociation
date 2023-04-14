using ElyriaAlumniAssociation.Data;
using ElyriaAlumniAssociation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElyriaAlumniAssociation.Controllers
{
    public class DeletedAlumniController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<DeletedAlumniController> _logger;
        private readonly DeletedAlumnusDbContext _deletedContext;

        public DeletedAlumniController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<DeletedAlumniController> logger, DeletedAlumnusDbContext deletedContext)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _deletedContext = deletedContext;
        }
        public async Task <IActionResult> Index()
        {
            var results = await _deletedContext.DeletedAlumnus.ToListAsync();
            return View(results);
        }

        public async Task<IActionResult> RestoreAlumni(List<DeletedAlumnus>? alumni)
        {
            List<DeletedAlumnus> alumniToRestore = new List<DeletedAlumnus>();
            foreach (var alumnus in alumni)
            {
                if (alumnus.Selected)
                {
                    var foundAlumnus = await _deletedContext.DeletedAlumnus.FirstOrDefaultAsync(x => x.Id == alumnus.Id);
                    var restoredAlumnus = ConvertDeletedAlumnusToAlumnus(foundAlumnus);
                    _context.Alumnus.Add(restoredAlumnus);
                    _deletedContext.DeletedAlumnus.Remove(_deletedContext.DeletedAlumnus.Find(alumnus.Id));
                }
            }

            await _deletedContext.SaveChangesAsync();
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteAlumni(List<DeletedAlumnus>? alumni)
        {
            List<DeletedAlumnus> alumniToDelete = new List<DeletedAlumnus>();
            foreach (var alumnus in alumni)
            {
                if (alumnus.Selected)
                {
                    alumniToDelete.Add(alumnus);
                }
            }

            _deletedContext.DeletedAlumnus.RemoveRange(alumniToDelete);
            await _deletedContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private Alumnus ConvertDeletedAlumnusToAlumnus(DeletedAlumnus alumnus)
        {
            Alumnus restoredAlumnus = new Alumnus();
            restoredAlumnus.FirstName = alumnus.FirstName;
            restoredAlumnus.LastName = alumnus.LastName;
            restoredAlumnus.MiddleInitial = alumnus.MiddleInitial;
            restoredAlumnus.LastNameAtGraduation = alumnus.LastNameAtGraduation;
            restoredAlumnus.School = alumnus.School;
            restoredAlumnus.GraduationYear = alumnus.GraduationYear;
            restoredAlumnus.StreetAddress = alumnus.StreetAddress;
            restoredAlumnus.City = alumnus.City;
            restoredAlumnus.State = alumnus.State;
            restoredAlumnus.Country = alumnus.Country;
            restoredAlumnus.PostalCode = alumnus.PostalCode;
            restoredAlumnus.EmailAddress = alumnus.EmailAddress;
            restoredAlumnus.PhoneNumber = alumnus.PhoneNumber;
            restoredAlumnus.ScholasticAward = alumnus.ScholasticAward;
            restoredAlumnus.Athletics = alumnus.Athletics;
            restoredAlumnus.Theatre = alumnus.Theatre;
            restoredAlumnus.Band = alumnus.Band;
            restoredAlumnus.Choir = alumnus.Choir;
            restoredAlumnus.Clubs = alumnus.Clubs;
            restoredAlumnus.ClassOfficer = alumnus.ClassOfficer;
            restoredAlumnus.ROTC = alumnus.ROTC;
            restoredAlumnus.OtherActivities = alumnus.OtherActivities;
            restoredAlumnus.CurrentStatus = alumnus.CurrentStatus;
            restoredAlumnus.Selected = false;

            return restoredAlumnus;
        }
    }
}
