using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterLogin.Domain.Entities;

namespace StarterLogin.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50);

        // Mapeo del Value Object Email
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(100);
            
            email.HasIndex(e => e.Value).IsUnique();
        });

        // Mapeo del Value Object PasswordHash
        builder.OwnsOne(u => u.PasswordHash, password =>
        {
            password.Property(p => p.Value)
                .HasColumnName("PasswordHash")
                .IsRequired();
        });

        builder.Property(u => u.IsActive)
            .IsRequired();

        // Muchos a muchos con Roles
        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity(j => j.ToTable("UserRoles"));
    }
}
