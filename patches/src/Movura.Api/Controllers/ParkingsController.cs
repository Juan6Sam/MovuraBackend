using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Movura.Api.Services;
using Movura.Api.Models.Dto;
using System.Threading.Tasks;

namespace Movura.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("parkings")]
    public class ParkingsController : ControllerBase
    {
        private readonly IParkingsService _svc;
        public ParkingsController(IParkingsService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] bool activeOnly = false, [FromQuery] int page=1, [FromQuery] int pageSize=50)
        {
            var list = await _svc.GetParkingsAsync(activeOnly, page, pageSize);
            return Ok(new { success = true, data = list });
        }

        [HttpGet("{parkingId}")]
        public async Task<IActionResult> GetDetail(int parkingId)
        {
            var p = await _svc.GetParkingDetailAsync(parkingId);
            if (p == null) return NotFound(new { success = false, error = new { code = "NOT_FOUND", message = "Parking no existe" } });
            return Ok(new { success = true, data = p });
        }

        [HttpPut("{parkingId}/config")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UpdateConfig(int parkingId, [FromBody] ParkingConfigRequest req)
        {
            var res = await _svc.UpdateConfigAsync(parkingId, req);
            if (!res.Success) return BadRequest(new { success = false, error = new { code = "INVALID", message = res.Message } });
            return Ok(new { success = true, data = res.Config });
        }
    }
}
