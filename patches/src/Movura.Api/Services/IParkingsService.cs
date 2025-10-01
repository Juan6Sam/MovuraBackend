using System.Collections.Generic;
using System.Threading.Tasks;
using Movura.Api.Models.Dto;

namespace Movura.Api.Services
{
    public interface IParkingsService
    {
        Task<List<ParkingListItem>> GetParkingsAsync(bool activeOnly, int page, int pageSize);
        Task<ParkingDetail> GetParkingDetailAsync(int parkingId);
        Task<ParkingConfigResult> UpdateConfigAsync(int parkingId, ParkingConfigRequest req);
    }
}
