namespace FitProgress.Infrastructure.Services
{
    public class UserProfileService
    {
        private readonly Supabase.Client _supabase;

        public UserProfileService(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public Supabase.Client Client => _supabase;
    }
}