using FitProgress.Application.DTOs.PhysicalRecords;
using FitProgress.Application.PhysicalRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitProgress.Api.Controllers
{
    [ApiController]
    [Route("api/physical-records")]
    public class PhysicalRecordsController : ControllerBase
    {
        private readonly IPhysicalRecordService _physicalRecordService;

        public PhysicalRecordsController(IPhysicalRecordService physicalRecordService)
        {
            _physicalRecordService = physicalRecordService;
        }
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRecord(CreatePhysicalRecordRequestDTO request)
        {

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return Unauthorized(new
                {
                    message = "Usuário não autenticado."
                });
            }

            var userId = Guid.Parse(userIdClaim);

            var response = await _physicalRecordService.CreateAsync(userId, request);

            if (!response.Success)
            {
                return BadRequest(new
                {
                    message = response.Message
                });
            }

            return StatusCode(201, response.Data);
        }
    }
}