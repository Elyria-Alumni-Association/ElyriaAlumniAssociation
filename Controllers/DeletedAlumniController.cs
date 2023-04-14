using ElyriaAlumniAssociation.Data;
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

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _deletedContext.DeletedAlumnus == null)
            {
                return NotFound();
            }

            var alumnus = await _deletedContext.DeletedAlumnus
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
            if (_deletedContext.DeletedAlumnus == null)
            {
                return Problem("Entity set 'DeletedAlumnusDbContext.DeletedAlumnus'  is null.");
            }
            var alumnus = await _deletedContext.DeletedAlumnus.FindAsync(id);
         
            if (alumnus != null)
            {
                _deletedContext.DeletedAlumnus.Remove(alumnus);
            }

            await _deletedContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
