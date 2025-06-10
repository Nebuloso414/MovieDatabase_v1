using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieDatabase.Models;
using System.Reflection.Emit;

namespace MovieDatabase.EntityConfigurations
{
    public class MovieCastConfiguration : IEntityTypeConfiguration<MovieCast>
    {
        public void Configure(EntityTypeBuilder<MovieCast> modelBuilder)
        {
            modelBuilder.ToTable("MovieCasts");

            modelBuilder.HasKey(mc => new { mc.MovieId, mc.PersonId, mc.RoleId });

            // Relationships with explicit navigation properties
            modelBuilder.HasOne(mc => mc.Movie)
                .WithMany(m => m.Cast)
                .HasForeignKey(mc => mc.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.HasOne(mc => mc.Person)
                .WithMany(p => p.Filmography)
                .HasForeignKey(mc => mc.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.HasOne(mc => mc.Role)
                .WithMany(r => r.Credits)
                .HasForeignKey(mc => mc.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Default dates
            modelBuilder.Property(mc => mc.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Property(mc => mc.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            // Initial table seed
            modelBuilder.HasData(new MovieCast[]
            {
                new MovieCast
                {
                    MovieId = 1,
                    PersonId = 1,
                    RoleId = 1
                },
                new MovieCast
                {
                    MovieId = 1,
                    PersonId = 2,
                    RoleId = 1
                },
                new MovieCast
                {
                    MovieId = 1,
                    PersonId = 13,
                    RoleId = 2
                },
                new MovieCast
                {
                    MovieId = 2,
                    PersonId = 5,
                    RoleId = 1
                },
                new MovieCast
                {
                    MovieId = 2,
                    PersonId = 6,
                    RoleId = 1
                },
                new MovieCast
                {
                    MovieId = 2,
                    PersonId = 11,
                    RoleId = 2
                },
                new MovieCast
                {
                    MovieId = 2,
                    PersonId = 12,
                    RoleId = 2
                },
                new MovieCast
                {
                    MovieId = 3,
                    PersonId = 8,
                    RoleId = 1
                },
                new MovieCast
                {
                    MovieId = 3,
                    PersonId = 9,
                    RoleId = 1
                },
                new MovieCast
                {
                    MovieId = 3,
                    PersonId = 10,
                    RoleId = 2
                },
            });
        }
    }
}
