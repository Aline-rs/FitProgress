using FitProgress.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitProgress.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupabaseTestController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public SupabaseTestController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        [HttpGet("profiles")]
        public async Task<IActionResult> GetProfiles()
        {
            try
            {
                var response = await _supabase
                    .From<Profile>()
                    .Get();

                var result = response.Models.Select(x => new
                {
                    x.Id,
                    x.Name
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "Erro ao conectar no Supabase.",
                    error = ex.Message
                });
            }
        }
    }
}