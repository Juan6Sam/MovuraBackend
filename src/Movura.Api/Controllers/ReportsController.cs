using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movura.Api.Constants;
using Movura.Api.Models.Dto;
using Movura.Api.Services.Interfaces;

namespace Movura.Api.Controllers;

[ApiController]
[Route("api/parkings/{parkingId}/[controller]")]
[Authorize(Roles = $"{UserRoles.Admin},{UserRoles.ParkingAdmin}")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(
        IReportService reportService,
        ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _logger = logger;
    }

    [HttpGet("occupancy")]
    public async Task<ActionResult<OccupancyReportDto>> GetOccupancyReport(
        string parkingId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        _logger.LogInformation(
            "Generando reporte de ocupación para el parking {ParkingId} desde {StartDate} hasta {EndDate}",
            parkingId, startDate, endDate);

        var report = await _reportService.GetOccupancyReportAsync(parkingId, startDate, endDate);
        return Ok(report);
    }

    [HttpGet("transactions")]
    public async Task<ActionResult<List<TransactionReportDto>>> GetTransactionReport(
        string parkingId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        _logger.LogInformation(
            "Generando reporte de transacciones para el parking {ParkingId} desde {StartDate} hasta {EndDate}",
            parkingId, startDate, endDate);

        var report = await _reportService.GetTransactionReportAsync(parkingId, startDate, endDate);
        return Ok(report);
    }

    [HttpGet("comercios")]
    public async Task<ActionResult<Dictionary<string, ComercioReportDto>>> GetComercioReport(
        string parkingId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        _logger.LogInformation(
            "Generando reporte de comercios para el parking {ParkingId} desde {StartDate} hasta {EndDate}",
            parkingId, startDate, endDate);

        var report = await _reportService.GetComercioReportAsync(parkingId, startDate, endDate);
        return Ok(report);
    }

    [HttpGet("occupancy/export")]
    public async Task<ActionResult> ExportOccupancyReport(
        string parkingId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string format = "excel")
    {
        _logger.LogInformation(
            "Exportando reporte de ocupación para el parking {ParkingId} en formato {Format}",
            parkingId, format);

        var reportBytes = await _reportService.ExportOccupancyReportAsync(parkingId, startDate, endDate, format);

        var contentType = format.ToLower() switch
        {
            "excel" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "csv" => "text/csv",
            _ => throw new ArgumentException("Formato no soportado", nameof(format))
        };

        var fileName = $"occupancy-report-{parkingId}-{startDate:yyyyMMdd}-{endDate:yyyyMMdd}.{(format == "excel" ? "xlsx" : "csv")}";

        return File(reportBytes, contentType, fileName);
    }

    [HttpGet("transactions/export")]
    public async Task<ActionResult> ExportTransactionReport(
        string parkingId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string format = "excel")
    {
        _logger.LogInformation(
            "Exportando reporte de transacciones para el parking {ParkingId} en formato {Format}",
            parkingId, format);

        var reportBytes = await _reportService.ExportTransactionReportAsync(parkingId, startDate, endDate, format);

        var contentType = format.ToLower() switch
        {
            "excel" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "csv" => "text/csv",
            _ => throw new ArgumentException("Formato no soportado", nameof(format))
        };

        var fileName = $"transaction-report-{parkingId}-{startDate:yyyyMMdd}-{endDate:yyyyMMdd}.{(format == "excel" ? "xlsx" : "csv")}";

        return File(reportBytes, contentType, fileName);
    }
}