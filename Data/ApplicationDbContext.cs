using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ElyriaAlumniAssociation.Models;

namespace ElyriaAlumniAssociation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ElyriaAlumniAssociation.Models.Alumnus>? Alumnus { get; set; }
    }
}