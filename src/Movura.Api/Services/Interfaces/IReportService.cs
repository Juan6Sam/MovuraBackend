using Movura.Api.Models.Dto;

namespace Movura.Api.Services.Interfaces;

public interface IReportService
{
    Task<OccupancyReportDto> GetOccupancyReportAsync(string parkingId, DateTime startDate, DateTime endDate);
    Task<List<TransactionReportDto>> GetTransactionReportAsync(string parkingId, DateTime startDate, DateTime endDate);
    Task<byte[]> ExportTransactionReportAsync(string parkingId, DateTime startDate, DateTime endDate, string format);
    Task<byte[]> ExportOccupancyReportAsync(string parkingId, DateTime startDate, DateTime endDate, string format);
    Task<Dictionary<string, ComercioReportDto>> GetComercioReportAsync(string parkingId, DateTime startDate, DateTime endDate);
}