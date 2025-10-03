using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movura.Api.Data.Context;
using Movura.Domain.Entities;
using Movura.Api.Models.Dto;
using Movura.Api.Services.Interfaces;

namespace Movura.Api.Services;

// El servicio ha sido re-escrito para alinearse con la estructura real de la base de datos.
// Se ha eliminado toda la lógica que hacía referencia a la colección 'Comercio.Usuarios',
// ya que no existe una relación directa entre Comercios y Usuarios en la base de datos.
public class ComercioService : IComercioService
{
    private readonly MovuraDbContext _context;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<ComercioService> _logger;

    public ComercioService(
        MovuraDbContext context,
        IMapper mapper,
        IEmailService emailService,
        ILogger<ComercioService> logger)
    {
        _context = context;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<List<ComercioDto>> GetByParkingIdAsync(string parkingId)
    {
        var comercios = await _context.Set<Comercio>()
            .Where(c => c.ParkingId == parkingId)
            .ToListAsync();

        return _mapper.Map<List<ComercioDto>>(comercios);
    }

    public async Task<ComercioDto> CreateAsync(string parkingId, ComercioDto comercioDto)
    {
        var parking = await ValidateParkingAsync(parkingId);

        var comercio = _mapper.Map<Comercio>(comercioDto);
        comercio.ParkingId = parkingId;

        ValidateComercio(comercio);

        _context.Set<Comercio>().Add(comercio);
        await _context.SaveChangesAsync();

        await NotifyNewComercioAsync(comercio, parking.AdminCorreo);

        return _mapper.Map<ComercioDto>(comercio);
    }

    public async Task<ComercioDto> UpdateAsync(string parkingId, int comercioId, ComercioDto comercioDto)
    {
        var comercio = await _context.Set<Comercio>()
            .FirstOrDefaultAsync(c => c.Id == comercioId && c.ParkingId == parkingId)
            ?? throw new InvalidOperationException($"No se encontró el comercio con ID {comercioId}");

        _mapper.Map(comercioDto, comercio);
        ValidateComercio(comercio);

        await _context.SaveChangesAsync();

        return _mapper.Map<ComercioDto>(comercio);
    }

    public async Task DeleteAsync(string parkingId, int comercioId)
    {
        var comercio = await _context.Set<Comercio>()
            .FirstOrDefaultAsync(c => c.Id == comercioId && c.ParkingId == parkingId)
            ?? throw new InvalidOperationException($"No se encontró el comercio con ID {comercioId}");

        _context.Set<Comercio>().Remove(comercio);
        await _context.SaveChangesAsync();

        // Opcional: Notificar al admin sobre la eliminación, ya no se puede notificar a los usuarios.
        // await NotifyDeletedComercioAsync(comercio);
    }

    public async Task<List<ComercioDto>> BulkUpdateAsync(string parkingId, List<ComercioDto> comercios)
    {
        await ValidateParkingAsync(parkingId);

        var currentComercios = await _context.Set<Comercio>()
            .Where(c => c.ParkingId == parkingId)
            .ToListAsync();

        var comerciosToDelete = currentComercios
            .Where(c => !comercios.Any(dto => dto.Id == c.Id))
            .ToList();

        _context.Set<Comercio>().RemoveRange(comerciosToDelete);

        var updatedComercios = new List<Comercio>();
        foreach (var comercioDto in comercios)
        {
            Comercio comercio;
            if (comercioDto.Id == 0)
            {
                comercio = _mapper.Map<Comercio>(comercioDto);
                comercio.ParkingId = parkingId;
                _context.Set<Comercio>().Add(comercio);
            }
            else
            {
                comercio = currentComercios.First(c => c.Id == comercioDto.Id);
                _mapper.Map(comercioDto, comercio);
            }
            ValidateComercio(comercio);
            updatedComercios.Add(comercio);
        }

        await _context.SaveChangesAsync();
        return _mapper.Map<List<ComercioDto>>(updatedComercios);
    }

    public async Task NotifyAccountsAsync(string parkingId, int comercioId, List<string> accounts)
    {
        var comercio = await _context.Set<Comercio>()
            .FirstOrDefaultAsync(c => c.Id == comercioId && c.ParkingId == parkingId)
            ?? throw new InvalidOperationException($"No se encontró el comercio con ID {comercioId}");

        foreach (var account in accounts)
        {
            await _emailService.SendEmailAsync(
                account,
                $"Notificación de Comercio - {comercio.Nombre}",
                $"Se te ha enviado una notificación del comercio {comercio.Nombre}."
            );
        }
    }

    private async Task<Parking> ValidateParkingAsync(string parkingId)
    {
        var parking = await _context.Set<Parking>().FindAsync(parkingId);
        if (parking == null)
        {
            throw new InvalidOperationException($"No se encontró el parking con ID {parkingId}");
        }
        return parking;
    }

    private void ValidateComercio(Comercio comercio)
    {
        if (comercio.Tipo != "monto" && comercio.Tipo != "tiempo")
        {
            throw new InvalidOperationException("El tipo de comercio debe ser 'monto' o 'tiempo'");
        }

        if (comercio.Valor <= 0)
        {
            throw new InvalidOperationException("El valor debe ser mayor que 0");
        }
    }

    private async Task NotifyNewComercioAsync(Comercio comercio, string adminEmail)
    {
        var emailBody = $@"
            <h2>Nuevo Comercio Creado</h2>
            <p>Se ha creado un nuevo comercio con los siguientes detalles:</p>
            <ul>
                <li>Nombre: {comercio.Nombre}</li>
                <li>Tipo: {comercio.Tipo}</li>
                <li>Valor: {comercio.Valor}</li>
            </ul>";

        await _emailService.SendEmailAsync(adminEmail, $"Nuevo Comercio - {comercio.Nombre}", emailBody);
    }
}
