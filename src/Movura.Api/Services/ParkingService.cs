using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Movura.Api.Data.Context;
using Movura.Api.Data.Entities;
using Movura.Api.Models.Dto;
using Movura.Api.Services.Interfaces;

namespace Movura.Api.Services;

public class ParkingService : IParkingService
{
    private readonly MovuraDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<ParkingService> _logger;

    public ParkingService(MovuraDbContext context, IMapper mapper, ILogger<ParkingService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<ParkingDto>> GetAllAsync(bool activeOnly = false)
    {
        try
        {
            var query = _context.Parkings
                .Include(p => p.Config)
                .Include(p => p.Comercios)
                .AsQueryable();

            if (activeOnly)
            {
                query = query.Where(p => p.Estatus == "Activo");
            }

            var parkings = await query.ToListAsync();
            return _mapper.Map<List<ParkingDto>>(parkings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener lista de parkings");
            throw;
        }
    }

    public async Task<ParkingDto?> GetByIdAsync(string id)
    {
        try
        {
            var parking = await _context.Parkings
                .Include(p => p.Config)
                .Include(p => p.Comercios)
                .FirstOrDefaultAsync(p => p.Id == id);

            return parking != null ? _mapper.Map<ParkingDto>(parking) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener parking {ParkingId}", id);
            throw;
        }
    }

    public async Task<bool> UpdateConfigAsync(string id, ParkingConfigDto configDto)
    {
        try
        {
            var parking = await _context.Parkings
                .Include(p => p.Config)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (parking == null)
            {
                throw new InvalidOperationException($"No se encontró el parking con ID {id}");
            }

            // Si no existe configuración, crear una nueva
            if (parking.Config == null)
            {
                parking.Config = new ParkingConfig
                {
                    ParkingId = parking.Id
                };
            }

            // Actualizar la configuración
            _mapper.Map(configDto, parking.Config);

            // Validar la configuración
            ValidateParkingConfig(parking.Config);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar configuración del parking {ParkingId}", id);
            throw;
        }
    }

    private void ValidateParkingConfig(ParkingConfig config)
    {
        if (config.TarifaBase < 0)
            throw new InvalidOperationException("La tarifa base no puede ser negativa");

        if (config.CostoHora < 0)
            throw new InvalidOperationException("El costo por hora no puede ser negativo");

        if (config.FraccionMin <= 0)
            throw new InvalidOperationException("La fracción mínima debe ser mayor a 0 minutos");

        if (config.CostoFraccion < 0)
            throw new InvalidOperationException("El costo por fracción no puede ser negativo");

        if (config.GraciaMin < 0)
            throw new InvalidOperationException("El tiempo de gracia no puede ser negativo");

        // Validar formato de hora (HH:mm)
        if (!TimeSpan.TryParse(config.HoraCorte, out _))
            throw new InvalidOperationException("El formato de hora de corte debe ser HH:mm");
    }
}