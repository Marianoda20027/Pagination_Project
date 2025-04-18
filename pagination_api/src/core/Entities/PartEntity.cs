using System;

namespace PaginationApp.Core.Entities
{
    public class Part
    {
        public int Id { get; set; }
        public DateTime ProductionDate { get; set; }        
        public DateTime LastModified { get; set; }          
        public int StockQuantity { get; set; }             
        public decimal UnitWeight { get; set; }            
        public string PartCode { get; set; }              
        public string Category { get; set; }               
        public string TechnicalSpecs { get; set; }
    }
}