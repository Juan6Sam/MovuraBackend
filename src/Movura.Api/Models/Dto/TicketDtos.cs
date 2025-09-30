namespace Movura.Api.Models.Dto;

public class TicketDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public DateTime EntradaISO { get; set; }
    public DateTime? SalidaISO { get; set; }
    public string Status { get; set; } = null!;
    public decimal? EstimatedAmount { get; set; }
}

public class TransactionDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime EntradaISO { get; set; }
    public DateTime SalidaISO { get; set; }
    public int Minutes { get; set; }
    public string Status { get; set; } = null!;
    public decimal Monto { get; set; }
    public decimal Excedente { get; set; }
    public decimal Total { get; set; }
}

public class MarkPaidRequest
{
    public string? OperatorId { get; set; }
    public decimal Amount { get; set; }
    public string? Method { get; set; }
    public string? Notes { get; set; }
}

public class MarkPaidResponse
{
    public bool Success { get; set; }
    public string TicketId { get; set; } = null!;
    public string QrToken { get; set; } = null!;
    public string QrUrl { get; set; } = null!;
    public DateTime IssuedAtISO { get; set; }
    public DateTime ExpiresAtISO { get; set; }
}

public class PagedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public PageMetadata Meta { get; set; } = null!;
}

public class PageMetadata
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
}