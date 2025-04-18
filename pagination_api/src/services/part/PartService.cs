using PaginationApp.Core.Entities;
using PaginationApp.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using PaginationApp.Core.Models;
using PaginationApp.Services.Parts.Contracts;

namespace PaginationApp.Services.Parts
{
    public class PartService
    {
        private readonly IPartSearchService _partSearchService;
        private readonly ILogger<PartService> _logger;

        public PartService(
            IPartSearchService partSearchService,
            ILogger<PartService> logger)
        {
            _partSearchService = partSearchService;
            _logger = logger;
        }

        public async Task<PaginatedResult<PartDto>> GetPaginatedPartsAsync(
            int pageNumber,
            int pageSize,
            Dictionary<string, string>? filters = null)
        {
            return await _partSearchService.SearchPartsAsync(pageNumber, pageSize, filters);
        }
    }
}
