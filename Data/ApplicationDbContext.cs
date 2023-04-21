using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ElyriaAlumniAssociation.Models;
using ElyriaAlumniAssociation.Utils;

namespace ElyriaAlumniAssociation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DeletedAlumnus>? DeletedAlumnus { get; set; }
        public DbSet<Alumnus>? Alumnus { get; set; }

    }
}