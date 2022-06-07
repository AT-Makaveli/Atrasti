namespace Atrasti.Search
{
    public class SearchDocument
    {
        public string DocumentId { get; set; }
        
        public int CompanyId { get; set; }
        
        public string Title { get; set; }

        public string[] Tags { get; set; }
        
        public string Description { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }

        public string Country { get; set; }
        
        public string BusinessType { get; set; }

        public string MainProducts { get; set; }

        public string MainMarkets { get; set; }
        
        public string Certificates { get; set; }
        
        public string Category { get; set; }
    }
}