using FitProgress.Infrastructure.Configurations;

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