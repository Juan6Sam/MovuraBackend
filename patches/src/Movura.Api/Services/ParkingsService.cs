using System.Collections.Generic;
using System.Threading.Tasks;
using Movura.Api.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Movura.Api.Data;
using System.Linq;
using System;

namespace Movura.Api.Services
{
    public class ParkingsService : IParkingsService
    {
        private readonly MovuraDbContext _db;
        public ParkingsService(MovuraDbContext db) { _db = db; }

        public async Task<List<ParkingListItem>> GetParkingsAsync(bool activeOnly, int page, int pageSize)
        {
            var q = _db.Parkings.AsQueryable();
            if (activeOnly) q = q.Where(p => p.Status == "active");
            var items = await q.Skip((page-1)*pageSize).Take(pageSize).Select(p => new ParkingListItem {
                Id = p.Id, Nombre = p.Nombre, Direccion = p.Direccion, Grupo = p.Grupo, AdminNombre = p.AdminNombre, AdminCorreo = p.AdminCorreo, AltaISO = p.Alta, Estatus = p.Status
            }).ToListAsync();
            return items;
        }

        public async Task<ParkingDetail> GetParkingDetailAsync(int parkingId)
        {
            var p = await _db.Parkings.Include(p=>p.Comercios).FirstOrDefaultAsync(x=>x.Id==parkingId);
            if (p == null) return null;
            return new ParkingDetail { Id = p.Id, Nombre = p.Nombre, Config = p.ConfigJson, Comercios = p.Comercios.Select(c=> new { c.Id, c.Nombre }).ToList() };
        }

        public async Task<ParkingConfigResult> UpdateConfigAsync(int parkingId, ParkingConfigRequest req)
        {
            if (req.TarifaBase < 0 || req.CostoHora < 0) return new ParkingConfigResult { Success = false, Message = "Valores invalidos" };
            var p = await _db.Parkings.FindAsync(parkingId);
            if (p==null) return new ParkingConfigResult { Success = false, Message = "No existe parking" };
            p.ConfigJson = System.Text.Json.JsonSerializer.Serialize(req);
            await _db.SaveChangesAsync();
            return new ParkingConfigResult { Success = true, Config = req };
        }
    }
}
