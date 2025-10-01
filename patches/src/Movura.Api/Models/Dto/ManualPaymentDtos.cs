using System;

namespace Movura.Api.Models.Dto
{
    public class ManualPaymentRequest
    {
        public int OperatorId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } // "manual"
        public string Notes { get; set; }
    }

    public class ManualPaymentResult
    {
        public bool Success { get; set; }
        public int TicketId { get; set; }
        public string QrToken { get; set; }
        public string QrUrl { get; set; }
        public DateTime IssuedAtISO { get; set; }
        public DateTime ExpiresAtISO { get; set; }
        public string Message { get; set; }
    }
}
