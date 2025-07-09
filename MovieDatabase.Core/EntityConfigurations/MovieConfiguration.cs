using MovieDatabase.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MovieDatabase.Core.EntityConfigurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> modelBuilder)
        {
            //builder.ToTable("Movies");
            modelBuilder
                .HasKey(m => m.Id);
            
            modelBuilder
                .HasMany(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieGenre",
                    j => j.HasData(
                        new { MoviesId = 1, GenresId = 1 },
                        new { MoviesId = 1, GenresId = 5 },
                        new { MoviesId = 2, GenresId = 1 },
                        new { MoviesId = 2, GenresId = 5 },
                        new { MoviesId = 3, GenresId = 3 }
                    ));

            modelBuilder
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder
                .Property(m => m.ReleaseDate)
                .IsRequired();

            modelBuilder
                .Property(m => m.Rating)
                .HasColumnType("decimal(4,2)")
                .IsRequired();

            modelBuilder.Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Property(r => r.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder
                .HasData(new Movie
                {
                    Id = 1,
                    Title = "Inception",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    Length = 148,
                    Rating = 8.8m
                },
                new Movie
                {
                    Id = 2,
                    Title = "The Matrix",
                    ReleaseDate = new DateTime(1999, 3, 31),
                    Length = 136,
                    Rating = 8.7m
                },
                new Movie
                {
                    Id = 3,
                    Title = "The Shawshank Redemption",
                    ReleaseDate = new DateTime(1994, 9, 23),
                    Length = 142,
                    Rating = 9.3m
                });
        }
    }
}
