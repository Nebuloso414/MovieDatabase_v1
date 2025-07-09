using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieDatabase.Core.Models;

namespace MovieDatabase.Core.EntityConfigurations
{
    public class PeopleConfigurations : IEntityTypeConfiguration<People>
    {
        public void Configure(EntityTypeBuilder<People> modelBuilder)
        {
            modelBuilder.ToTable("People");
            modelBuilder.HasKey(p => p.Id);

            modelBuilder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Property(p => p.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Property(p => p.UpdatedDate)
                            .HasDefaultValueSql("GETDATE()");

            modelBuilder.HasData(
                new People { Id = 1, FirstName = "Leonardo", LastName = "DiCaprio", DateOfBirth = new DateTime(1974, 11, 11) },
                new People { Id = 2, FirstName = "Joseph", LastName = "Gordon-Levitt", DateOfBirth = new DateTime(1981, 02, 17) },
                new People { Id = 3, FirstName = "Emma", LastName = "Stone", DateOfBirth = new DateTime(1988, 11, 06) },
                new People { Id = 4, FirstName = "Natalie", LastName = "Portman", DateOfBirth = new DateTime(1981, 06, 09) },
                new People { Id = 5, FirstName = "Keanu", LastName = "Reeves", DateOfBirth = new DateTime(1964, 09, 02) },
                new People { Id = 6, FirstName = "Laurence", LastName = "Fishburne", DateOfBirth = new DateTime(1961, 07, 30) },
                new People { Id = 7, FirstName = "Keanu", LastName = "Reeves", DateOfBirth = new DateTime(1964, 09, 02) },
                new People { Id = 8, FirstName = "Tim", LastName = "Robbins", DateOfBirth = new DateTime(1958, 10, 16) },
                new People { Id = 9, FirstName = "Morgan", LastName = "Freeman", DateOfBirth = new DateTime(1937, 06, 01) },
                new People { Id = 10, FirstName = "Frank", LastName = "Darabont", DateOfBirth = new DateTime(1959, 01, 28) },
                new People { Id = 11, FirstName = "Lana", LastName = "Wachowski", DateOfBirth = new DateTime(1965, 06, 21) },
                new People { Id = 12, FirstName = "Lilly", LastName = "Wachowski", DateOfBirth = new DateTime(1967, 12, 29) },
                new People { Id = 13, FirstName = "Christopher", LastName = "Nolan", DateOfBirth = new DateTime(1970, 07, 30) }
            );
        }
    }
}
