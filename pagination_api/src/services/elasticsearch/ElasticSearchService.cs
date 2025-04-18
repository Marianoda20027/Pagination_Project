using Elasticsearch.Net;
using PaginationApp.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaginationApp.Services.ElasticSearch
{
    public class ElasticSearchService
    {
        private readonly ElasticConnection _connection;

        public ElasticSearchService(ElasticConnection connection)
        {
            _connection = connection;
        }

        public async Task<string> SearchPartsAsync(Dictionary<string, string> filters, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) 
                throw new BadRequestException("Número de página inválido (debe ser ≥ 1)");
            if (pageSize < 1 || pageSize > 100) 
                throw new BadRequestException("Tamaño de página inválido (debe ser 1-100)");

            var mustClauses = new List<object>();

            if (filters == null || filters.Count == 0)
            {
                mustClauses.Add(new { match_all = new { } });
            }
            else
            {
                foreach (var filter in filters)
                {
                    switch (filter.Key.ToLower())
                    {
                        case "productiondatestart":
                        case "productiondateend":
                            if (filters.ContainsKey("productiondatestart") && filters.ContainsKey("productiondateend"))
                            {
                                mustClauses.Add(new
                                {
                                    range = new Dictionary<string, object>
                                    {
                                        ["productiondate"] = new
                                        {
                                            gte = filters["productiondatestart"],
                                            lte = filters["productiondateend"]
                                        }
                                    }
                                });
                            }
                            break;

                        case "lastmodifiedstart":
                        case "lastmodifiedend":
                            if (filters.ContainsKey("lastmodifiedstart") && filters.ContainsKey("lastmodifiedend"))
                            {
                                mustClauses.Add(new
                                {
                                    range = new Dictionary<string, object>
                                    {
                                        ["lastmodified"] = new
                                        {
                                            gte = filters["lastmodifiedstart"],
                                            lte = filters["lastmodifiedend"]
                                        }
                                    }
                                });
                            }
                            break;

                        case "minstockquantity":
                        case "maxstockquantity":
                            if (filters.ContainsKey("minstockquantity") && filters.ContainsKey("maxstockquantity"))
                            {
                                if (!int.TryParse(filters["minstockquantity"], out _) || !int.TryParse(filters["maxstockquantity"], out _))
                                    throw new BadRequestException("StockQuantity debe ser un número entero");

                                mustClauses.Add(new
                                {
                                    range = new Dictionary<string, object>
                                    {
                                        ["stockquantity"] = new
                                        {
                                            gte = int.Parse(filters["minstockquantity"]),
                                            lte = int.Parse(filters["maxstockquantity"])
                                        }
                                    }
                                });
                            }
                            break;

                        case "minunitweight":
                        case "maxunitweight":
                            if (filters.ContainsKey("minunitweight") && filters.ContainsKey("maxunitweight"))
                            {
                                if (!decimal.TryParse(filters["minunitweight"], out _) || !decimal.TryParse(filters["maxunitweight"], out _))
                                    throw new BadRequestException("UnitWeight debe ser un número decimal");

                                mustClauses.Add(new
                                {
                                    range = new Dictionary<string, object>
                                    {
                                        ["unitweight"] = new
                                        {
                                            gte = decimal.Parse(filters["minunitweight"]),
                                            lte = decimal.Parse(filters["maxunitweight"])
                                        }
                                    }
                                });
                            }
                            break;

                        case "partcode":
                            mustClauses.Add(new
                            {
                                term = new Dictionary<string, object>
                                {
                                    ["partcode"] = filters["partcode"]
                                }
                            });
                            break;

                        case "category":
                            mustClauses.Add(new
                            {
                                match = new Dictionary<string, object>
                                {
                                    ["category"] = new
                                    {
                                        query = filters["category"],
                                        fuzziness = "AUTO"
                                    }
                                }
                            });
                            break;

                        case "technicalspecs":
                            mustClauses.Add(new
                            {
                                match = new Dictionary<string, object>
                                {
                                    ["technicalspecs"] = new
                                    {
                                        query = filters["technicalspecs"],
                                        fuzziness = "AUTO"
                                    }
                                }
                            });
                            break;
                    }
                }
            }

            var query = new
            {
                query = new { @bool = new { must = mustClauses } },
                from = (pageNumber - 1) * pageSize,
                size = pageSize,
                _source = new[] { "id", "partcode", "category", "stockquantity", "unitweight", "productiondate", "lastmodified", "technicalspecs", "@timestamp", "tags" }
            };

            var response = await _connection.Client.SearchAsync<StringResponse>("parts", PostData.Serializable(query));

            if (!response.Success)
                throw new ElasticsearchException("Error al realizar la búsqueda");

            return response.Body;
        }
    }
}