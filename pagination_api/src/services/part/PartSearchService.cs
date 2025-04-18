using PaginationApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaginationApp.Services.ElasticSearch;
using PaginationApp.Services.Parts.Contracts;

namespace PaginationApp.Services.Parts
{
    public class ElasticPartSearchService : IPartSearchService
    {
        private readonly ElasticSearchService _searchService;
        private readonly IPartMapper _mapper;

        public ElasticPartSearchService(
            ElasticSearchService searchService, 
            IPartMapper mapper)
        {
            _searchService = searchService;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<PartDto>> SearchPartsAsync(int pageNumber, int pageSize, Dictionary<string, string> filters = null)
        {
            var response = await _searchService.SearchPartsAsync(filters ?? new Dictionary<string, string>(), pageNumber, pageSize);
            return _mapper.MapToPaginatedResult(response, pageNumber, pageSize);
        }
    }
}