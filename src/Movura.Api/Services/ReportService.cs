using System.Text;
using ClosedXML.Excel;
using Movura.Api.Data.Context;
using Movura.Api.Data.Entities; // Added this to resolve entity properties
using Movura.Api.Models.Dto;
using Movura.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Movura.Api.Services;

public class ReportService : IReportService
{
    private readonly MovuraDbContext _context;
    private readonly ILogger<ReportService> _logger;

    public ReportService(
        MovuraDbContext context,
        ILogger<ReportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OccupancyReportDto> GetOccupancyReportAsync(string parkingId, DateTime startDate, DateTime endDate)
    {
        var parking = await _context.Parkings // This should work now
            .Include(p => p.Config)
            .FirstOrDefaultAsync(p => p.Id == parkingId)
            ?? throw new InvalidOperationException($"No se encontró el parking con ID {parkingId}");

        var totalSpaces = parking.Config?.TotalSpaces ?? 0;

        var tickets = await _context.Tickets
            .Where(t => t.ParkingId == parkingId && // This should work now
                       t.EntryTime >= startDate && // This should work now
                       (t.ExitTime == null || t.ExitTime <= endDate)) // This should work now
            .ToListAsync();

        var hourlyOccupancy = new List<OccupancyDataPoint>();
        var currentTime = startDate;

        while (currentTime <= endDate)
        {
            var occupiedSpaces = tickets.Count(t =>
                t.EntryTime <= currentTime &&
                (t.ExitTime == null || t.ExitTime > currentTime));

            var occupancyRate = totalSpaces > 0 ? (double)occupiedSpaces / totalSpaces * 100 : 0;

            hourlyOccupancy.Add(new OccupancyDataPoint
            {
                DateTime = currentTime,
                OccupiedSpaces = occupiedSpaces,
                OccupancyRate = Math.Round(occupancyRate, 2)
            });

            currentTime = currentTime.AddHours(1);
        }

        var peakOccupancy = hourlyOccupancy.MaxBy(o => o.OccupiedSpaces);
        var averageOccupancyRate = hourlyOccupancy.Average(o => o.OccupancyRate);

        return new OccupancyReportDto
        {
            TotalSpaces = totalSpaces,
            HourlyOccupancy = hourlyOccupancy,
            AverageOccupancyRate = Math.Round(averageOccupancyRate, 2),
            PeakOccupancy = peakOccupancy?.OccupiedSpaces ?? 0,
            PeakOccupancyTime = peakOccupancy?.DateTime ?? DateTime.MinValue
        };
    }

    public async Task<List<TransactionReportDto>> GetTransactionReportAsync(string parkingId, DateTime startDate, DateTime endDate)
    {
        return await _context.Transacciones // Corrected from Transactions
            .Where(t => t.ParkingId == parkingId &&
                       t.CreatedAt >= startDate &&
                       t.CreatedAt <= endDate)
            .OrderBy(t => t.CreatedAt)
            .Select(t => new TransactionReportDto
            {
                TicketId = t.TicketId.ToString(),
                EntryTime = t.Ticket!.EntryTime,
                ExitTime = t.Ticket.ExitTime,
                Amount = t.Amount,
                PaymentMethod = t.PaymentMethod,
                ComercioId = t.ComercioId.ToString(),
                ComercioName = t.Comercio != null ? t.Comercio.Nombre : null,
                ComercioDiscount = t.DiscountAmount
            })
            .ToListAsync();
    }

    public async Task<Dictionary<int, ComercioReportDto>> GetComercioReportAsync(string parkingId, DateTime startDate, DateTime endDate)
    {
        var transactions = await _context.Transacciones // Corrected from Transactions
            .Where(t => t.ParkingId == parkingId &&
                       t.CreatedAt >= startDate &&
                       t.CreatedAt <= endDate &&
                       t.ComercioId != null)
            .Include(t => t.Comercio)
            .ToListAsync();

        var comercioReports = new Dictionary<int, ComercioReportDto>();

        foreach (var transaction in transactions.Where(t => t.Comercio != null))
        {
            var comercioId = transaction.ComercioId!.Value;
            if (!comercioReports.ContainsKey(comercioId))
            {
                comercioReports[comercioId] = new ComercioReportDto
                {
                    TotalTransactions = 0,
                    TotalDiscountAmount = 0,
                    Transactions = new List<ComercioTransactionDto>()
                };
            }

            var report = comercioReports[comercioId];
            report.TotalTransactions++;
            report.TotalDiscountAmount += transaction.DiscountAmount;
            report.Transactions.Add(new ComercioTransactionDto
            {
                TicketId = transaction.TicketId.ToString(),
                TransactionTime = transaction.CreatedAt,
                DiscountAmount = transaction.DiscountAmount,
                DiscountType = transaction.Comercio!.Tipo
            });
        }

        return comercioReports;
    }

    public async Task<byte[]> ExportTransactionReportAsync(string parkingId, DateTime startDate, DateTime endDate, string format)
    {
        var transactions = await GetTransactionReportAsync(parkingId, startDate, endDate);

        return format.ToLower() switch
        {
            "excel" => GenerateExcelTransactionReport(transactions),
            "csv" => GenerateCsvTransactionReport(transactions),
            _ => throw new ArgumentException("Formato no soportado. Use 'excel' o 'csv'.", nameof(format))
        };
    }

    public async Task<byte[]> ExportOccupancyReportAsync(string parkingId, DateTime startDate, DateTime endDate, string format)
    {
        var occupancyReport = await GetOccupancyReportAsync(parkingId, startDate, endDate);

        return format.ToLower() switch
        {
            "excel" => GenerateExcelOccupancyReport(occupancyReport),
            "csv" => GenerateCsvOccupancyReport(occupancyReport),
            _ => throw new ArgumentException("Formato no soportado. Use 'excel' o 'csv'.", nameof(format))
        };
    }

    private static byte[] GenerateExcelTransactionReport(List<TransactionReportDto> transactions)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Transactions");

        worksheet.Cell(1, 1).Value = "Ticket ID";
        worksheet.Cell(1, 2).Value = "Entrada";
        worksheet.Cell(1, 3).Value = "Salida";
        worksheet.Cell(1, 4).Value = "Monto";
        worksheet.Cell(1, 5).Value = "Método de Pago";
        worksheet.Cell(1, 6).Value = "Comercio";
        worksheet.Cell(1, 7).Value = "Descuento";

        var row = 2;
        foreach (var transaction in transactions)
        {
            worksheet.Cell(row, 1).Value = transaction.TicketId;
            worksheet.Cell(row, 2).Value = transaction.EntryTime;
            worksheet.Cell(row, 3).Value = transaction.ExitTime;
            worksheet.Cell(row, 4).Value = transaction.Amount;
            worksheet.Cell(row, 5).Value = transaction.PaymentMethod;
            worksheet.Cell(row, 6).Value = transaction.ComercioName ?? "";
            worksheet.Cell(row, 7).Value = transaction.ComercioDiscount ?? 0;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static byte[] GenerateExcelOccupancyReport(OccupancyReportDto report)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Occupancy");

        worksheet.Cell(1, 1).Value = "Espacios Totales";
        worksheet.Cell(1, 2).Value = report.TotalSpaces;
        worksheet.Cell(2, 1).Value = "Ocupación Promedio (%)";
        worksheet.Cell(2, 2).Value = report.AverageOccupancyRate;
        worksheet.Cell(3, 1).Value = "Ocupación Máxima";
        worksheet.Cell(3, 2).Value = report.PeakOccupancy;
        worksheet.Cell(4, 1).Value = "Hora de Máxima Ocupación";
        worksheet.Cell(4, 2).Value = report.PeakOccupancyTime;

        worksheet.Cell(6, 1).Value = "Fecha/Hora";
        worksheet.Cell(6, 2).Value = "Espacios Ocupados";
        worksheet.Cell(6, 3).Value = "Tasa de Ocupación (%)";

        var row = 7;
        foreach (var dataPoint in report.HourlyOccupancy)
        {
            worksheet.Cell(row, 1).Value = dataPoint.DateTime;
            worksheet.Cell(row, 2).Value = dataPoint.OccupiedSpaces;
            worksheet.Cell(row, 3).Value = dataPoint.OccupancyRate;
            row++;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static byte[] GenerateCsvTransactionReport(List<TransactionReportDto> transactions)
    {
        var csv = new StringBuilder(); // This should work now
        csv.AppendLine("TicketId,Entrada,Salida,Monto,Método de Pago,Comercio,Descuento");

        foreach (var transaction in transactions)
        {
            csv.AppendLine($"{transaction.TicketId}," +
                          $"{transaction.EntryTime:yyyy-MM-dd HH:mm:ss}," +
                          $"{transaction.ExitTime:yyyy-MM-dd HH:mm:ss}," +
                          $"{transaction.Amount}," +
                          $"{transaction.PaymentMethod}," +
                          $"{transaction.ComercioName}," +
                          $"{transaction.ComercioDiscount}");
        }

        return Encoding.UTF8.GetBytes(csv.ToString()); // This should work now
    }

    private static byte[] GenerateCsvOccupancyReport(OccupancyReportDto report)
    {
        var csv = new StringBuilder(); // This should work now
        
        csv.AppendLine($"Espacios Totales,{report.TotalSpaces}");
        csv.AppendLine($"Ocupación Promedio (%),{report.AverageOccupancyRate}");
        csv.AppendLine($"Ocupación Máxima,{report.PeakOccupancy}");
        csv.AppendLine($"Hora de Máxima Ocupación,{report.PeakOccupancyTime:yyyy-MM-dd HH:mm:ss}");
        csv.AppendLine();
        csv.AppendLine("Fecha/Hora,Espacios Ocupados,Tasa de Ocupación (%)");

        foreach (var dataPoint in report.HourlyOccupancy)
        {
            csv.AppendLine($"{dataPoint.DateTime:yyyy-MM-dd HH:mm:ss}," +
                          $"{dataPoint.OccupiedSpaces}," +
                          $"{dataPoint.OccupancyRate}");
        }

        return Encoding.UTF8.GetBytes(csv.ToString()); // This should work now
    }
}
