using PaginationApp.Core.Models;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using System;
using PaginationApp.Services.Parts.Contracts;

namespace PaginationApp.Services.Parts
{

    public class PartMapper : IPartMapper
    {
        public PaginatedResult<PartDto> MapToPaginatedResult(string elasticResponse, int pageNumber, int pageSize)
        {
            using var jsonDoc = JsonDocument.Parse(elasticResponse);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("hits", out var hits))
                throw new InvalidOperationException("Invalid Elasticsearch response format: missing 'hits' property");

            long total = 0;
            if (hits.TryGetProperty("total", out var totalProp) && 
                totalProp.TryGetProperty("value", out var totalValue))
            {
                total = totalValue.GetInt64();
            }

            var items = new List<PartDto>();

            if (hits.TryGetProperty("hits", out var hitsArray))
            {
                foreach (var hit in hitsArray.EnumerateArray())
                {
                    if (!hit.TryGetProperty("_source", out var source))
                        continue;

                    var dto = new PartDto
                    {
                        Id = source.TryGetProperty("id", out var idProp) ? idProp.GetInt32() : 0,
                        PartCode = source.TryGetProperty("partcode", out var codeProp) ? codeProp.GetString() : null,
                        Category = source.TryGetProperty("category", out var categoryProp) ? categoryProp.GetString() : null,
                        StockQuantity = source.TryGetProperty("stockquantity", out var stockProp) ? stockProp.GetInt32() : 0,
                        UnitWeight = source.TryGetProperty("unitweight", out var weightProp) ? weightProp.GetDecimal() : 0m,
                        TechnicalSpecs = source.TryGetProperty("technicalspecs", out var specsProp) ? specsProp.GetString() : null
                    };

                    if (source.TryGetProperty("productiondate", out var prodDateProp) && 
                        DateTime.TryParse(prodDateProp.GetString(), out var productionDate))
                    {
                        dto.ProductionDate = productionDate;
                    }
                    
                    if (source.TryGetProperty("lastmodified", out var modDateProp) && 
                        DateTime.TryParse(modDateProp.GetString(), out var lastModified))
                    {
                        dto.LastModified = lastModified;
                    }

                    items.Add(dto);
                }
            }

            return new PaginatedResult<PartDto>
            {
                Items = items,
                Total = total,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}