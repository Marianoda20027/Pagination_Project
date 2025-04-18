using Microsoft.AspNetCore.Mvc;
using PaginationApp.Core.Exceptions;
using PaginationApp.Core.Models;
using PaginationApp.Services.Parts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaginationApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartsController : ControllerBase
    {
        private readonly PartService _partService;
        private readonly ILogger<PartsController> _logger;

        public PartsController(PartService partService, ILogger<PartsController> logger)
        {
            _partService = partService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaginatedParts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string category = null,
            [FromQuery] string partCode = null,
            [FromQuery] int? minStock = null,
            [FromQuery] int? maxStock = null,
            [FromQuery] string productionDateStart = null,
            [FromQuery] string productionDateEnd = null)
        {
            try
            {
                _logger.LogInformation($"Solicitud recibida. Página: {pageNumber}, Tamaño: {pageSize}");

                if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
                {
                    _logger.LogWarning("Parámetros de paginación inválidos");
                    throw new BadRequestException("Parámetros de paginación inválidos");
                }

                var filters = new Dictionary<string, string>();
                
                if (!string.IsNullOrEmpty(category))
                    filters.Add("Category", category);
                    
                if (!string.IsNullOrEmpty(partCode))
                    filters.Add("PartCode", partCode);
                    
                if (minStock.HasValue)
                    filters.Add("MinStockQuantity", minStock.Value.ToString());
                    
                if (maxStock.HasValue)
                    filters.Add("MaxStockQuantity", maxStock.Value.ToString());
                    
                if (!string.IsNullOrEmpty(productionDateStart))
                    filters.Add("ProductionDateStart", productionDateStart);
                    
                if (!string.IsNullOrEmpty(productionDateEnd))
                    filters.Add("ProductionDateEnd", productionDateEnd);

                var result = await _partService.GetPaginatedPartsAsync(pageNumber, pageSize, filters);
                
                if (result.Total == 0)
                {
                    _logger.LogInformation("No se encontraron resultados");
                    return NotFound(new { Message = "No se encontraron resultados con los criterios de búsqueda" });
                }

                _logger.LogInformation($"Devolviendo {result.Items.Count} de {result.Total} resultados");
                return Ok(result);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno del servidor");
                return StatusCode(500, new { Message = "Error interno del servidor" });
            }
        }
    }
}