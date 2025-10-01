using Movura.Api.Data;
using Movura.Api.Models.Dto;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Movura.Api.Services
{
    public class ReportsService : IReportsService
    {
        private readonly MovuraDbContext _db;
        public ReportsService(MovuraDbContext db) { _db = db; }

        public async Task<PagedResult<OccupancyItem>> GetOccupancyAsync(int parkingId, string start, string end, int page, int pageSize)
        {
            var s = DateTime.Parse(start);
            var e = DateTime.Parse(end);
            var q = _db.Tickets.Where(t=>t.ParkingId==parkingId && t.Entrada >= s && t.Entrada <= e);
            var total = await q.CountAsync();
            var items = await q.Skip((page-1)*pageSize).Take(pageSize).Select(t=> new OccupancyItem { Id = t.Id, Email = t.Email, EntradaISO = t.Entrada, SalidaISO = t.Salida, Status = t.Status }).ToListAsync();
            return new PagedResult<OccupancyItem> { Items = items, Meta = new Meta { Page = page, PageSize = pageSize, Total = total } };
        }

        public async Task<PagedResult<TransactionsItem>> GetTransactionsAsync(int parkingId, string start, string end, int page, int pageSize)
        {
            var s = DateTime.Parse(start);
            var e = DateTime.Parse(end);
            var q = _db.Transactions.Where(t=>t.ParkingId==parkingId && t.IssuedAt >= s && t.IssuedAt <= e);
            var total = await q.CountAsync();
            var items = await q.Skip((page-1)*pageSize).Take(pageSize).Select(t=> new TransactionsItem { Id = t.Id, Minutes = t.Minutes, Status = t.Status, Monto = t.Amount, Excedente = t.Extra, Total = t.Total }).ToListAsync();
            return new PagedResult<TransactionsItem> { Items = items, Meta = new Meta { Page = page, PageSize = pageSize, Total = total } };
        }

        public async Task<List<object>> ManualSearchAsync(int parkingId, string start, string email, string phone)
        {
            var s = DateTime.Parse(start);
            var q = _db.Tickets.Where(t=>t.ParkingId==parkingId && t.Entrada >= s && (t.Email==email || t.Phone==phone) && t.Status!="paid");
            return await q.Select(t=> new { t.Id, t.Email, t.Phone, EstimatedAmount = 0 }).Cast<object>().ToListAsync();
        }

        public async Task<(bool Success, ManualPaymentResult Data, string Message)> MarkPaidAsync(int parkingId, int ticketId, ManualPaymentRequest req)
        {
            var t = await _db.Tickets.FirstOrDefaultAsync(x=>x.Id==ticketId && x.ParkingId==parkingId);
            if (t==null) return (false, null, "Ticket no encontrado");
            if (t.Status=="paid") return (false, null, "Ticket ya pagado");

            var trx = new Data.Entities.Transaccion {
                ParkingId = parkingId,
                TicketId = ticketId,
                Amount = req.Amount,
                Method = req.Method,
                OperatorId = req.OperatorId,
                IssuedAt = DateTime.UtcNow,
                Status = "pagado",
                Minutes = (int)((t.Salida ?? DateTime.UtcNow) - t.Entrada).TotalMinutes,
                Extra = 0,
                Total = req.Amount
            };
            _db.Transactions.Add(trx);
            t.Status = "paid";
            await _db.SaveChangesAsync();

            var token = Guid.NewGuid().ToString("N");
            var url = $"https://example.com/qr/{token}";
            var now = DateTime.UtcNow;
            var res = new ManualPaymentResult { Success = true, TicketId = ticketId, QrToken = token, QrUrl = url, IssuedAtISO = now, ExpiresAtISO = now.AddHours(24) };
            return (true, res, null);
        }
}
