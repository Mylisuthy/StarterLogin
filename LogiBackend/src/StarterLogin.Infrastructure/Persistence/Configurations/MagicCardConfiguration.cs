using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterLogin.Domain.Entities;

namespace StarterLogin.Infrastructure.Persistence.Configurations;

public class MagicCardConfiguration : IEntityTypeConfiguration<MagicCard>
{
    public void Configure(EntityTypeBuilder<MagicCard> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.ImageUrl)
            .IsRequired();
            
        builder.Property(c => c.IsPublished)
            .IsRequired();
    }
}
