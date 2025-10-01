using System.Collections.Generic;
using System.Threading.Tasks;
using Movura.Api.Models.Dto;

namespace Movura.Api.Services
{
    public interface IComerciosService
    {
        Task<List<ComercioDto>> ListAsync(int parkingId);
        Task<int> CreateAsync(int parkingId, ComercioCreateRequest req);
        Task<ComercioDto> GetByIdAsync(int parkingId, int comercioId);
        Task<bool> UpdateAsync(int parkingId, int comercioId, ComercioCreateRequest req);
        Task<bool> DeleteAsync(int parkingId, int comercioId);
        Task<ComerciosBulkResult> BulkReplaceAsync(int parkingId, List<ComercioCreateRequest> comercios);
    }
}
