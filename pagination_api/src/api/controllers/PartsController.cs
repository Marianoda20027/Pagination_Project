using Microsoft.AspNetCore.Mvc;
using PaginationApp.Core.Exceptions;
using PaginationApp.Core.Models;
using PaginationApp.Services.Parts;
using Microsoft.Extensions.Logging;
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
            [FromQuery] string? category = null,
            [FromQuery] string? partCode = null,
            [FromQuery] string? technicalSpecs = null,
            [FromQuery] int? minStock = null,
            [FromQuery] int? maxStock = null,
            [FromQuery] decimal? minUnitWeight = null,
            [FromQuery] decimal? maxUnitWeight = null,
            [FromQuery] string? productionDateStart = null,
            [FromQuery] string? productionDateEnd = null,
            [FromQuery] string? lastModifiedStart = null,
            [FromQuery] string? lastModifiedEnd = null)
        {

            if (pageNumber < 1) 
                throw new BadRequestException("Número de página inválido (debe ser ≥ 1)");
            if (pageSize < 1 || pageSize > 100) 
                throw new BadRequestException("Tamaño de página inválido (debe ser 1-100)");

            var filters = new Dictionary<string, string>();
            
            if (!string.IsNullOrEmpty(category))
                filters.Add("Category", category);
                
            if (!string.IsNullOrEmpty(partCode))
                filters.Add("PartCode", partCode);
                
            if (!string.IsNullOrEmpty(technicalSpecs))
                filters.Add("TechnicalSpecs", technicalSpecs);
                
            if (minStock.HasValue)
                filters.Add("MinStockQuantity", minStock.Value.ToString());
                
            if (maxStock.HasValue)
                filters.Add("MaxStockQuantity", maxStock.Value.ToString());
                
            if (minUnitWeight.HasValue)
                filters.Add("MinUnitWeight", minUnitWeight.Value.ToString());
                
            if (maxUnitWeight.HasValue)
                filters.Add("MaxUnitWeight", maxUnitWeight.Value.ToString());
                
            if (!string.IsNullOrEmpty(productionDateStart))
                filters.Add("ProductionDateStart", productionDateStart);
                
            if (!string.IsNullOrEmpty(productionDateEnd))
                filters.Add("ProductionDateEnd", productionDateEnd);
                
            if (!string.IsNullOrEmpty(lastModifiedStart))
                filters.Add("LastModifiedStart", lastModifiedStart);
                
            if (!string.IsNullOrEmpty(lastModifiedEnd))
                filters.Add("LastModifiedEnd", lastModifiedEnd);

            var result = await _partService.GetPaginatedPartsAsync(pageNumber, pageSize, filters);
            
            if (result.Total == 0)
            {
                _logger.LogInformation("No se encontraron resultados");
            }

            return Ok(result);
        }
    }
}