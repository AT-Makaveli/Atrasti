namespace Atrasti.API.Models.ProductModels
{
    public class UploadProduct_Req
    {
        public string Image { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string[] Tags { get; set; }
        
        public int Category { get; set; }
    }
}