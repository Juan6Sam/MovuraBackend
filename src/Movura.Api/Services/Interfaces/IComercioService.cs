using Movura.Api.Models.Dto;

namespace Movura.Api.Services.Interfaces;

public interface IComercioService
{
    Task<List<ComercioDto>> GetByParkingIdAsync(string parkingId);
    Task<ComercioDto> CreateAsync(string parkingId, ComercioDto comercioDto);
    Task<ComercioDto> UpdateAsync(string parkingId, string comercioId, ComercioDto comercioDto);
    Task DeleteAsync(string parkingId, string comercioId);
    Task<List<ComercioDto>> BulkUpdateAsync(string parkingId, List<ComercioDto> comercios);
    Task NotifyAccountsAsync(string parkingId, string comercioId, List<string> accounts);
}