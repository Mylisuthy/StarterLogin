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
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Roles.AnyAsync()) return;

        // Crear Roles
        var adminRole = Role.Create("Admin", "System Administrator");
        var userRole = Role.Create("User", "Standard User");

        await context.Roles.AddRangeAsync(adminRole, userRole);

        // Crear Admin User (Password temporal, debe ser hasheado en la realidad)
        // Por ahora lo guardamos como un hash plano para simplificar la fase 2
        var adminUser = User.Create(
            "admin", 
            Email.Create("admin@starterlogin.com"), 
            PasswordHash.Create("Admin123!") // En la fase 3 implementaremos hashing real
        );
        adminUser.AddRole(adminRole);

        await context.Users.AddAsync(adminUser);

        await context.SaveChangesAsync();
    }
}
