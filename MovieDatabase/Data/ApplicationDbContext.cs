using Microsoft.EntityFrameworkCore;
using MovieDatabase.Models;

namespace MovieDatabase.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<People> People { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<MovieCast> MovieCasts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityConfigurations.MovieConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.GenreConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.PeopleConfigurations());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.RoleConfiguration());
            modelBuilder.ApplyConfiguration(new EntityConfigurations.MovieCastConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
