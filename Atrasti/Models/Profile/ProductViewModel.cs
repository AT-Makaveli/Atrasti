using System.Collections.Generic;

namespace Atrasti.Models.Profile
{
    public class ProductViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public IList<string> Tags { get; set; }
    }
}
