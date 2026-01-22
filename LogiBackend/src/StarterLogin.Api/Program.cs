using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StarterLogin.Application;
using StarterLogin.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5900")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// JWT Authentication Configuration
var secretKey = builder.Configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "StarterLogin API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (System.IO.File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<StarterLogin.Api.Middleware.ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<StarterLogin.Infrastructure.Persistence.ApplicationDbContext>();
    
    int retries = 10;
    while (retries > 0)
    {
        try
        {
            var passwordHasher = services.GetRequiredService<StarterLogin.Application.Common.Interfaces.IPasswordHasher>();
            
            logger.LogInformation("Attempting to migrate and seed database... (Retries left: {Retries})", retries);
            await StarterLogin.Infrastructure.Persistence.DbInitializer.SeedAsync(context, passwordHasher);
            logger.LogInformation("Database migrated and seeded successfully.");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            if (retries == 0)
            {
                logger.LogCritical(ex, "Could not migrate and seed database after multiple attempts.");
                throw;
            }
            logger.LogWarning("Database not ready yet. Retrying in 3 seconds... Error: {Message}", ex.Message);
            await Task.Delay(3000);
        }
    }
}

// Habilitar Swagger siempre para transparencia (solicitado por el usuario)
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StarterLogin API V1");
    c.RoutePrefix = "swagger"; // Acceso en /swagger
});

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication(); // Añadido antes de Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
