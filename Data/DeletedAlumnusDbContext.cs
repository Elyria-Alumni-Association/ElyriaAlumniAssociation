using ElyriaAlumniAssociation.Models;
using Microsoft.EntityFrameworkCore;

namespace ElyriaAlumniAssociation.Data
{
    public class DeletedAlumnusDbContext : DbContext
    {
        public DeletedAlumnusDbContext(DbContextOptions<DeletedAlumnusDbContext> options)
            : base(options)
        {
        }
        public DbSet<DeletedAlumnus>? DeletedAlumnus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<DeletedAlumnus>().ToTable(nameof(DeletedAlumnus));
        }
    }
}
