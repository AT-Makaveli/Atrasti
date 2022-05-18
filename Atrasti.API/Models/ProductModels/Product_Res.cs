using System.Collections.Generic;

namespace Atrasti.API.Models.ProductModels
{
    public class Product_Res
    {
        public Product_Res(uint id, string title, string description, IList<string> tags, IList<int> productLikes, bool isHeartPressed)
        {
            Id = id;
            Title = title;
            Description = description;
            Tags = tags;
            ProductLikes = productLikes;
            IsHeartPressed = isHeartPressed;
        }

        public uint Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<string> Tags { get; set; }
        public IList<int> ProductLikes { get; set; }
        public bool IsHeartPressed { get; set; }
    }
}