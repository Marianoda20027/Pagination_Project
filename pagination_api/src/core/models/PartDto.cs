using System.Text.Json.Serialization;

public class PartDto
{
    [JsonPropertyName("category")]
    public string Category { get; set; }
    
    [JsonPropertyName("id")]
    public int Id { get; set; }
     
    [JsonPropertyName("lastmodified")]
    public DateTime LastModified { get; set; }

    [JsonPropertyName("partcode")]
    public string PartCode { get; set; }
    
    
    [JsonPropertyName("productiondate")]
    public DateTime ProductionDate { get; set; }
    
    
    [JsonPropertyName("stockquantity")]
    public int StockQuantity { get; set; }
    
    [JsonPropertyName("technicalspecs")]
    public string TechnicalSpecs { get; set; }

     [JsonPropertyName("unitweight")]
    public decimal UnitWeight { get; set; }
}
