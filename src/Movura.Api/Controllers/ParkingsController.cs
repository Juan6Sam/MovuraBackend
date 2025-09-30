using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movura.Api.Constants;
using Movura.Api.Models.Dto;
using Movura.Api.Services.Interfaces;

namespace Movura.Api.Controllers;

[ApiController]
[Route("api/parkings")]
[Authorize]
public class ParkingsController : ControllerBase
{
    private readonly IParkingService _parkingService;
    private readonly ILogger<ParkingsController> _logger;

    public ParkingsController(IParkingService parkingService, ILogger<ParkingsController> logger)
    {
        _parkingService = parkingService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la lista de todos los parkings
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ParkingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetParkings([FromQuery] bool activeOnly = false)
    {
        try
        {
            var parkings = await _parkingService.GetAllAsync(activeOnly);
            return Ok(parkings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener parkings");
            return StatusCode(500, new ErrorResponse { Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Obtiene un parking por su ID
    /// </summary>
    [HttpGet("{parkingId}")]
    [ProducesResponseType(typeof(ParkingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetParkingById(string parkingId)
    {
        try
        {
            var parking = await _parkingService.GetByIdAsync(parkingId);
            if (parking == null)
            {
                return NotFound(new ErrorResponse { Message = "Parking no encontrado" });
            }

            return Ok(parking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener parking {ParkingId}", parkingId);
            return StatusCode(500, new ErrorResponse { Message = "Error interno del servidor" });
        }
    }

    /// <summary>
    /// Actualiza la configuración de un parking
    /// </summary>
    [HttpPut("{parkingId}/config")]
    [Authorize(Policy = Policies.CanManageParkings)]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateParkingConfig(string parkingId, [FromBody] ParkingConfigDto config)
    {
        try
        {
            var result = await _parkingService.UpdateConfigAsync(parkingId, config);
            return Ok(new { success = result, config });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar configuración del parking {ParkingId}", parkingId);
            return StatusCode(500, new ErrorResponse { Message = "Error interno del servidor" });
        }
    }
}