using System.Collections.Generic;
using System.Threading.Tasks;
using Movura.Api.Models.Dto;
using Movura.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Movura.Api.Services
{
    public class ComerciosService : IComerciosService
    {
        private readonly MovuraDbContext _db;
        public ComerciosService(MovuraDbContext db) { _db = db; }

        public async Task<List<ComercioDto>> ListAsync(int parkingId)
        {
            var q = _db.Comercios.Where(c=>c.ParkingId==parkingId && c.Status!="deleted");
            return await q.Select(c=> new ComercioDto { Id = c.Id, Nombre = c.Nombre, Tipo = c.Tipo, Valor = c.Valor, Usuarios = c.UsuariosJson != null ? System.Text.Json.JsonSerializer.Deserialize<List<string>>(c.UsuariosJson) : new List<string>(), Estatus = c.Status }).ToListAsync();
        }

        public async Task<int> CreateAsync(int parkingId, ComercioCreateRequest req)
        {
            var c = new Data.Entities.Comercio {
                ParkingId = parkingId,
                Nombre = req.Nombre,
                Tipo = req.Tipo,
                Valor = req.Valor,
                UsuariosJson = System.Text.Json.JsonSerializer.Serialize(req.Usuarios ?? new List<string>()),
                Status = req.Estatus ?? "active"
            };
            _db.Comercios.Add(c);
            await _db.SaveChangesAsync();
            return c.Id;
        }

        public async Task<ComercioDto> GetByIdAsync(int parkingId, int comercioId)
        {
            var c = await _db.Comercios.FirstOrDefaultAsync(x=>x.Id==comercioId && x.ParkingId==parkingId);
            if (c==null) return null;
            return new ComercioDto { Id = c.Id, Nombre = c.Nombre, Tipo = c.Tipo, Valor = c.Valor, Usuarios = c.UsuariosJson != null ? System.Text.Json.JsonSerializer.Deserialize<List<string>>(c.UsuariosJson) : new List<string>(), Estatus = c.Status };
        }

        public async Task<bool> UpdateAsync(int parkingId, int comercioId, ComercioCreateRequest req)
        {
            var c = await _db.Comercios.FirstOrDefaultAsync(x=>x.Id==comercioId && x.ParkingId==parkingId);
            if (c==null) return false;
            c.Nombre = req.Nombre; c.Tipo = req.Tipo; c.Valor = req.Valor; c.UsuariosJson = System.Text.Json.JsonSerializer.Serialize(req.Usuarios ?? new List<string>()); c.Status = req.Estatus ?? c.Status;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int parkingId, int comercioId)
        {
            var c = await _db.Comercios.FirstOrDefaultAsync(x=>x.Id==comercioId && x.ParkingId==parkingId);
            if (c==null) return false;
            c.Status = "deleted";
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ComerciosBulkResult> BulkReplaceAsync(int parkingId, List<ComercioCreateRequest> comercios)
        {
            using var trx = await _db.Database.BeginTransactionAsync();
            try
            {
                var existing = _db.Comercios.Where(c=>c.ParkingId==parkingId).ToList();
                foreach(var e in existing) { e.Status = "deleted"; }
                await _db.SaveChangesAsync();

                var created = new List<ComercioDto>();
                foreach(var req in comercios)
                {
                    var id = await CreateAsync(parkingId, req);
                    var dto = await GetByIdAsync(parkingId, id);
                    created.Add(dto);
                }
                await trx.CommitAsync();
                return new ComerciosBulkResult { Success = true, Comercios = created };
            }
            catch(Exception ex)
            {
                await trx.RollbackAsync();
                return new ComerciosBulkResult { Success = false, Message = ex.Message };
            }
        }
    }
}
