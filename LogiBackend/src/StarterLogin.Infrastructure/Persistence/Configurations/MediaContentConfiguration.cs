using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterLogin.Domain.Entities;

namespace StarterLogin.Infrastructure.Persistence.Configurations;

public class MediaContentConfiguration : IEntityTypeConfiguration<MediaContent>
{
    public void Configure(EntityTypeBuilder<MediaContent> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.HasOne(m => m.Genre)
            .WithMany()
            .HasForeignKey(m => m.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        // TPH Configuration
        builder.HasDiscriminator<string>("ContentType")
            .HasValue<Movie>("Movie")
            .HasValue<Series>("Series")
            .HasValue<Documentary>("Documentary");

        // High Scale Indexes
        builder.HasIndex(m => m.Title);
        builder.HasIndex(m => m.GenreId);
    }
}
