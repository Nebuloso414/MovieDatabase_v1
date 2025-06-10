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
        }
    }
}
