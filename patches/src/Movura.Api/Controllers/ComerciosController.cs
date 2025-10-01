using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Movura.Api.Services;
using Movura.Api.Models.Dto;
using System.Threading.Tasks;

namespace Movura.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("parkings/{parkingId}/comercios")]
    public class ComerciosController : ControllerBase
    {
        private readonly IComerciosService _svc;
        public ComerciosController(IComerciosService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> List(int parkingId) =>
            Ok(new { success = true, data = await _svc.ListAsync(parkingId) });

        [HttpPost]
        public async Task<IActionResult> Create(int parkingId, [FromBody] ComercioCreateRequest req)
        {
            var id = await _svc.CreateAsync(parkingId, req);
            return CreatedAtAction(nameof(GetById), new { parkingId = parkingId, comercioId = id }, new { success = true, id = id });
        }

        [HttpGet("{comercioId}")]
        public async Task<IActionResult> GetById(int parkingId, int comercioId)
        {
            var c = await _svc.GetByIdAsync(parkingId, comercioId);
            if (c == null) return NotFound(new { success = false, error = new { code = "NOT_FOUND" } });
            return Ok(new { success = true, data = c });
        }

        [HttpPut("{comercioId}")]
        public async Task<IActionResult> Update(int parkingId, int comercioId, [FromBody] ComercioCreateRequest req)
        {
            var ok = await _svc.UpdateAsync(parkingId, comercioId, req);
            if (!ok) return BadRequest(new { success = false, error = new { code = "UPDATE_FAILED" } });
            return Ok(new { success = true });
        }

        [HttpDelete("{comercioId}")]
        public async Task<IActionResult> Delete(int parkingId, int comercioId)
        {
            var ok = await _svc.DeleteAsync(parkingId, comercioId);
            if (!ok) return BadRequest(new { success = false, error = new { code = "DELETE_FAILED" } });
            return Ok(new { success = true });
        }

        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkReplace(int parkingId, [FromBody] ComerciosBulkRequest req)
        {
            var result = await _svc.BulkReplaceAsync(parkingId, req.Comercios);
            if (!result.Success) return BadRequest(new { success = false, error = new { code = "BULK_FAILED", message = result.Message } });
            return Ok(new { success = true, data = result.Comercios });
        }
    }
}
