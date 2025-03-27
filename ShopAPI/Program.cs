using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShopAPI.Model.EFCore;
using ShopAPI.Model.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShopAPI.Model.Identity;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();

        // Инициализация базы данных
        using (var scope = serviceProvider.CreateScope())
        {
            var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            await identityContext.Database.MigrateAsync();

            var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
            await dbContext.Database.MigrateAsync();
        }

        // Пример использования
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var authService = serviceProvider.GetRequiredService<AuthService>();

        var user = new IdentityUser { UserName = "test@example.com", Email = "test@example.com" };
        var result = await userManager.CreateAsync(user, "P@ssw0rd");

        if (result.Succeeded)
        {
            Console.WriteLine("User created successfully!");

            // Генерируем токен
            var token = await authService.GenerateJwtToken(user);
            Console.WriteLine($"JWT Token: {token}");

            // Валидируем токен
            var isValid = await authService.ValidateToken(token);
            Console.WriteLine($"Is token valid? {isValid}");
        }
        else
        {
            Console.WriteLine("User creation failed:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- {error.Description}");
            }
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<Context>(options =>
            options.UseSqlite("Data Source=shop.db"));

        services.AddDbContext<IdentityContext>(options =>
            options.UseSqlite("Data Source=auth.db"));

        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        var jwtSettings = new JwtSettings
        {
            Secret = "your-32-character-secret-key-here", // Минимум 32 символа
            Issuer = "ConsoleApp",
            Audience = "ConsoleAppUsers",
            ExpiryInMinutes = 60
        };

        services.AddSingleton(jwtSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddTransient<AuthService>();
    }
}