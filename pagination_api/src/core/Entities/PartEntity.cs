namespace PaginationApp.Core.Entities
{
    public class Part
    {
        public int Id { get; set; }
        public DateTime ProductionDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public int StockQuantity { get; set; }
        public decimal UnitWeight { get; set; }
        public string PartCode { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string TechnicalSpecs { get; set; } = string.Empty;
    }
}