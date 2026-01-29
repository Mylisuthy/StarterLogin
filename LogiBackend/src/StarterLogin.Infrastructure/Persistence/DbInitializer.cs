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

        // Seed Genres
        var actionGenre = Genre.Create("Acción", "Películas y series de mucha adrenalina");
        var dramaGenre = Genre.Create("Drama", "Historias intensas y emocionales");
        var sciFiGenre = Genre.Create("Sci-Fi", "Ciencia ficción y realidades alternativas");
        var comedyGenre = Genre.Create("Comedia", "Para reír y pasar un buen rato");

        await context.Genres.AddRangeAsync(actionGenre, dramaGenre, sciFiGenre, comedyGenre);

        // Seed Media
        var matrix = Movie.Create(
            "The Matrix", 
            "Un programador descubre que la realidad es una simulación.", 
            sciFiGenre.Id, 
            "https://res.cloudinary.com/demo/image/upload/v1631234567/matrix.jpg", 
            "https://res.cloudinary.com/demo/video/upload/v1631234567/matrix.mp4", 
            TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(16)), 
            new DateTime(1999, 3, 31), 
            "PG-13"
        );

        var godfather = Movie.Create(
            "The Godfather", 
            "La historia de la familia Corleone.", 
            dramaGenre.Id, 
            "https://res.cloudinary.com/demo/image/upload/v1631234567/godfather.jpg", 
            null, 
            TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(55)), 
            new DateTime(1972, 3, 24), 
            "R"
        );

        var interstellar = Movie.Create(
            "Interstellar", 
            "Un equipo de exploradores viaja a través de un agujero de gusano.", 
            sciFiGenre.Id, 
            "https://res.cloudinary.com/demo/image/upload/v1631234567/interstellar.jpg", 
            null, 
            TimeSpan.FromHours(2).Add(TimeSpan.FromMinutes(49)), 
            new DateTime(2014, 11, 7), 
            "PG-13"
        );

        await context.MediaContents.AddRangeAsync(matrix, godfather, interstellar);

        await context.SaveChangesAsync();
    }
}
