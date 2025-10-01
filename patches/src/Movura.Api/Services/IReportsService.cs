using Movura.Api.Models.Dto;
using System.Threading.Tasks;

namespace Movura.Api.Services
{
    public interface IReportsService
    {
        Task<PagedResult<OccupancyItem>> GetOccupancyAsync(int parkingId, string start, string end, int page, int pageSize);
        Task<PagedResult<TransactionsItem>> GetTransactionsAsync(int parkingId, string start, string end, int page, int pageSize);
        Task<System.Collections.Generic.List<object>> ManualSearchAsync(int parkingId, string start, string email, string phone);
        Task<(bool Success, ManualPaymentResult Data, string Message)> MarkPaidAsync(int parkingId, int ticketId, ManualPaymentRequest req);
    }
}
