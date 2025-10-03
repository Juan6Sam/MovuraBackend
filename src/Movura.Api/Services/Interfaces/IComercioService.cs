using Movura.Api.Models.Dto;

namespace Movura.Api.Services.Interfaces;

public interface IComercioService
{
    Task<List<ComercioDto>> GetByParkingIdAsync(string parkingId);
    Task<ComercioDto> CreateAsync(string parkingId, ComercioDto comercioDto);
    Task<ComercioDto> UpdateAsync(string parkingId, int comercioId, ComercioDto comercioDto);
    Task DeleteAsync(string parkingId, int comercioId);
    Task<List<ComercioDto>> BulkUpdateAsync(string parkingId, List<ComercioDto> comercios);
    Task NotifyAccountsAsync(string parkingId, int comercioId, List<string> accounts);
}
