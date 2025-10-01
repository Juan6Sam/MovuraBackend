using System;
using System.Collections.Generic;

namespace Movura.Api.Models.Dto
{
    public class OccupancyItem
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime EntradaISO { get; set; }
        public DateTime? SalidaISO { get; set; }
        public string Status { get; set; }
    }

    public class TransactionsItem
    {
        public int Id { get; set; }
        public int Minutes { get; set; }
        public string Status { get; set; }
        public decimal Monto { get; set; }
        public decimal Excedente { get; set; }
        public decimal Total { get; set; }
    }

    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public Meta Meta { get; set; }
    }

    public class Meta { public int Page { get; set; } public int PageSize { get; set; } public int Total { get; set; } }
}
