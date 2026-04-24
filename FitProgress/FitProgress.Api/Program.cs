using FitProgress.Api.Services.Auth;
using FitProgress.Api.Settings;
using FitProgress.Infrastructure.Configurations;
using FitProgress.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FitProgress.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            var jwtSettings = builder.Configuration
                .GetSection("Jwt")
                .Get<JwtSettings>();

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

                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key)
                        )
                    };
                });

            builder.Services.AddAuthorization();

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