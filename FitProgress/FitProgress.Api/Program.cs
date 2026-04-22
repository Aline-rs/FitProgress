using FitProgress.Infrastructure.Configurations;
using FitProgress.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitProgress.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}