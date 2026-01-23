using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterLogin.Domain.Entities;

namespace StarterLogin.Infrastructure.Persistence.Configurations;

public class UserMediaHistoryConfiguration : IEntityTypeConfiguration<UserMediaHistory>
{
    public void Configure(EntityTypeBuilder<UserMediaHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.HasOne(h => h.User)
            .WithMany()
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.MediaContent)
            .WithMany()
            .HasForeignKey(h => h.MediaContentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(h => new { h.UserId, h.MediaContentId }).IsUnique();
    }
}
