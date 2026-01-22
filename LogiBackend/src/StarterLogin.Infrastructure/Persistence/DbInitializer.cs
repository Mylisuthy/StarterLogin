using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterLogin.Domain.Entities;
using StarterLogin.Domain.ValueObjects;
using StarterLogin.Infrastructure.Persistence;

namespace StarterLogin.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context, StarterLogin.Application.Common.Interfaces.IPasswordHasher passwordHasher)
    {
        await context.Database.MigrateAsync();

        if (await context.Roles.AnyAsync()) return;

        // Crear Roles
        var adminRole = Role.Create("Admin", "System Administrator");
        var userRole = Role.Create("User", "Standard User");

        await context.Roles.AddRangeAsync(adminRole, userRole);

        // Crear Admin User con password hasheado
        var adminUser = User.Create(
            "admin", 
            Email.Create("admin@starterlogin.com"), 
            PasswordHash.Create(passwordHasher.Hash("Admin123!"))
        );
        adminUser.AddRole(adminRole);

        await context.Users.AddAsync(adminUser);

        await context.SaveChangesAsync();
    }
}
