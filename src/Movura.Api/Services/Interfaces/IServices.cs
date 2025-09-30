using Movura.Api.Models.Dto;

namespace Movura.Api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthLoginResponse> LoginAsync(AuthLoginRequest request);
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<bool> ChangeFirstPasswordAsync(ChangeFirstPasswordRequest request);
}

public interface IParkingService
{
    Task<List<ParkingDto>> GetAllAsync(bool activeOnly = false);
    Task<ParkingDto?> GetByIdAsync(string id);
    Task<bool> UpdateConfigAsync(string id, ParkingConfigDto config);
}

// Interfaces específicas están definidas en archivos separados (IComercioService, IReportService)

public interface ITicketService
{
    Task<List<TicketDto>> SearchTicketsAsync(string parkingId, DateTime? start, string? email, string? phone);
    Task<MarkPaidResponse> MarkPaidAsync(string parkingId, string ticketId, MarkPaidRequest request);
}