using FitProgress.Application.PhysicalRecords.Interfaces;
using FitProgress.Application.Services.Auth;
using FitProgress.Application.Services.PhysicalRecords;
using FitProgress.Application.Settings;
using FitProgress.Application.Users.Interfaces;
using FitProgress.Infrastructure.Configurations;
using FitProgress.Infrastructure.Data;
using FitProgress.Infrastructure.Repositories.PhysicalRecords;
using FitProgress.Infrastructure.Repositories.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

namespace FitProgress.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers
            builder.Services.AddControllers();

            // Settings
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("Jwt"));

            var jwtSettings = builder.Configuration
                .GetSection("Jwt")
                .Get<JwtSettings>();

            // Database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPhysicalRecordRepository, PhysicalRecordRepository>();

            // Application Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IPhysicalRecordService, PhysicalRecordService>();
            // Authentication / Authorization
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings!.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                });

            builder.Services.AddAuthorization();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FitProgress API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });
            });

            var supabaseSettings = new SupabaseSettings
            {
                Url = builder.Configuration["Supabase:Url"] ?? string.Empty,
                Key = builder.Configuration["Supabase:Key"] ?? string.Empty
            };

            if (string.IsNullOrWhiteSpace(supabaseSettings.Url) ||
                string.IsNullOrWhiteSpace(supabaseSettings.Key))
            {
                throw new InvalidOperationException("As configurações do Supabase não foram preenchidas.");
            }

            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = false
            };

            var supabase = new Supabase.Client(
                supabaseSettings.Url,
                supabaseSettings.Key,
                options);

            await supabase.InitializeAsync();

            builder.Services.AddSingleton(supabase);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}