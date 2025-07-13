using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieDatabase.Core.Models;

namespace MovieDatabase.Core.EntityConfigurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> modelBuilder)
        {
            //builder.ToTable("Genres");

            modelBuilder
                .HasKey(g => g.Id);

            modelBuilder
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder
                .Property(g => g.Description)
                .HasMaxLength(255);

            modelBuilder.Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Property(r => r.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            // Create Genre seed data
            modelBuilder.HasData(
                new Genre { Id = 1, Name = "Action", Description = "Movies that will have you at the edge of your seat!" },
                new Genre { Id = 2, Name = "Comedy", Description = "Movies that will make you cry of laughter" },
                new Genre { Id = 3, Name = "Drama", Description = "Movies that will make you cry...but not of laughter" },
                new Genre { Id = 4, Name = "Horror", Description = "Movies that will make you turn on all the lights" },
                new Genre { Id = 5, Name = "Sci-Fi", Description = "Movies that will challenge your imagination" },
                new Genre { Id = 6, Name = "Fantasy", Description = "Movies with incredible stories and characters" },
                new Genre { Id = 7, Name = "Documentary", Description = "Movies that will educate you" },
                new Genre { Id = 8, Name = "Thriller", Description = "Movies that will keep you on the edge of your seat!" }
            );
        }
    }
}
