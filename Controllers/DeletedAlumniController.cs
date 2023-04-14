using ElyriaAlumniAssociation.Data;
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

        public DeletedAlumniController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<DeletedAlumniController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task <IActionResult> Index()
        {
            var results = await _context.DeletedAlumnus.ToListAsync();
            return View(results);
        }
    }
}
