using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieDatabase.Models;

namespace MovieDatabase.EntityConfigurations
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


        }
    }
}
