namespace Movura.Api.Models.Dto;

public class OccupancyReportDto
{
    public int TotalSpaces { get; set; }
    public List<OccupancyDataPoint> HourlyOccupancy { get; set; } = new();
    public double AverageOccupancyRate { get; set; }
    public int PeakOccupancy { get; set; }
    public DateTime PeakOccupancyTime { get; set; }
}

public class OccupancyDataPoint
{
    public DateTime DateTime { get; set; }
    public int OccupiedSpaces { get; set; }
    public double OccupancyRate { get; set; }
}

public class TransactionReportDto
{
    public string TicketId { get; set; } = string.Empty;
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string? ComercioId { get; set; }
    public string? ComercioName { get; set; }
    public decimal? ComercioDiscount { get; set; }
}

public class ComercioReportDto
{
    public int TotalTransactions { get; set; }
    public decimal TotalDiscountAmount { get; set; }
    public List<ComercioTransactionDto> Transactions { get; set; } = new();
}

public class ComercioTransactionDto
{
    public string TicketId { get; set; } = string.Empty;
    public DateTime TransactionTime { get; set; }
    public decimal DiscountAmount { get; set; }
    public string DiscountType { get; set; } = string.Empty;
}