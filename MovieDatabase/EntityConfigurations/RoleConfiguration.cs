using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieDatabase.Models;
using System.Reflection.Emit;

namespace MovieDatabase.EntityConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> modelBuilder)
        {
            modelBuilder.ToTable("Roles");

            modelBuilder
                .HasKey(r => r.Id);

            modelBuilder
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("varchar");

            modelBuilder
                .Property(r => r.Description)
                .HasMaxLength(1000)
                .HasColumnType("varchar");

            modelBuilder.Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Property(r => r.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.HasData(
                new Role { Id = 1, Name = "Actor", Description = "An actor in a movie" },
                new Role { Id = 2, Name = "Director", Description = "A director of a movie" },
                new Role { Id = 3, Name = "Producer", Description = "A producer of a movie" },
                new Role { Id = 4, Name = "Crew", Description = "A crew member of a movie" }
            );
        }
    }
}
