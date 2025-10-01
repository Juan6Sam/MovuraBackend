using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Movura.Api.Services;
using System.Threading.Tasks;

namespace Movura.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("parkings/{parkingId}/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _svc;
        public ReportsController(IReportsService svc) => _svc = svc;

        [HttpGet("occupancy")]
        public async Task<IActionResult> Occupancy(int parkingId, [FromQuery] string start, [FromQuery] string end, [FromQuery] int page=1, [FromQuery] int pageSize=50)
        {
            var res = await _svc.GetOccupancyAsync(parkingId, start, end, page, pageSize);
            return Ok(new { success = true, data = res.Items, meta = res.Meta });
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> Transactions(int parkingId, [FromQuery] string start, [FromQuery] string end, [FromQuery] int page=1, [FromQuery] int pageSize=50)
        {
            var res = await _svc.GetTransactionsAsync(parkingId, start, end, page, pageSize);
            return Ok(new { success = true, data = res.Items, meta = res.Meta });
        }

        [HttpGet("/parkings/{parkingId}/manual-search")]
        public async Task<IActionResult> ManualSearch(int parkingId, [FromQuery] string start, [FromQuery] string email, [FromQuery] string phone)
        {
            var items = await _svc.ManualSearchAsync(parkingId, start, email, phone);
            return Ok(new { success = true, data = items });
        }

        [HttpPost("/parkings/{parkingId}/tickets/{ticketId}/mark-paid")]
        public async Task<IActionResult> MarkPaid(int parkingId, int ticketId, [FromBody] ManualPaymentRequest req)
        {
            var res = await _svc.MarkPaidAsync(parkingId, ticketId, req);
            if (!res.Success) return BadRequest(new { success = false, error = new { code = "PAY_ERROR", message = res.Message } });
            return Ok(new { success = true, data = res.Data });
        }
    }
}
