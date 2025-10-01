using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movura.Api.Constants;
using Movura.Api.Models.Dto;
using Movura.Api.Services.Interfaces;

namespace Movura.Api.Controllers;

[ApiController]
[Route("api/parkings/{parkingId}/[controller]")]
[Authorize]
public class ComerciosController : ControllerBase
{
    private readonly IComercioService _comercioService;
    private readonly ILogger<ComerciosController> _logger;

    public ComerciosController(
        IComercioService comercioService,
        ILogger<ComerciosController> logger)
    {
        _comercioService = comercioService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.ParkingAdmin}")]
    public async Task<ActionResult<List<ComercioDto>>> GetByParkingId(string parkingId)
    {
        _logger.LogInformation("Obteniendo comercios para el parking {ParkingId}", parkingId);
        var comercios = await _comercioService.GetByParkingIdAsync(parkingId);
        return Ok(comercios);
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.ParkingAdmin}")]
    public async Task<ActionResult<ComercioDto>> Create(string parkingId, ComercioDto comercioDto)
    {
        _logger.LogInformation("Creando nuevo comercio para el parking {ParkingId}", parkingId);
        var comercio = await _comercioService.CreateAsync(parkingId, comercioDto);
        return CreatedAtAction(nameof(GetByParkingId), new { parkingId }, comercio);
    }

    [HttpPut("{comercioId}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.ParkingAdmin}")]
    public async Task<ActionResult<ComercioDto>> Update(string parkingId, string comercioId, ComercioDto comercioDto)
    {
        if (!int.TryParse(comercioId, out var id))
        {
            return BadRequest("El ID del comercio debe ser un número entero.");
        }
        _logger.LogInformation("Actualizando comercio {ComercioId} del parking {ParkingId}", id, parkingId);
        var comercio = await _comercioService.UpdateAsync(parkingId, id, comercioDto);
        return Ok(comercio);
    }

    [HttpDelete("{comercioId}")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.ParkingAdmin}")]
    public async Task<ActionResult> Delete(string parkingId, string comercioId)
    {
        if (!int.TryParse(comercioId, out var id))
        {
            return BadRequest("El ID del comercio debe ser un número entero.");
        }
        _logger.LogInformation("Eliminando comercio {ComercioId} del parking {ParkingId}", id, parkingId);
        await _comercioService.DeleteAsync(parkingId, id);
        return NoContent();
    }

    [HttpPut]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.ParkingAdmin}")]
    public async Task<ActionResult<List<ComercioDto>>> BulkUpdate(string parkingId, List<ComercioDto> comercios)
    {
        _logger.LogInformation("Actualizando masivamente comercios del parking {ParkingId}", parkingId);
        var updatedComercios = await _comercioService.BulkUpdateAsync(parkingId, comercios);
        return Ok(updatedComercios);
    }

    [HttpPost("{comercioId}/notify")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.ParkingAdmin}")]
    public async Task<ActionResult> NotifyAccounts(string parkingId, string comercioId, [FromBody] List<string> accounts)
    {
        if (!int.TryParse(comercioId, out var id))
        {
            return BadRequest("El ID del comercio debe ser un número entero.");
        }
        _logger.LogInformation("Enviando notificaciones a usuarios del comercio {ComercioId}", id);
        await _comercioService.NotifyAccountsAsync(parkingId, id, accounts);
        return Ok();
    }
}
