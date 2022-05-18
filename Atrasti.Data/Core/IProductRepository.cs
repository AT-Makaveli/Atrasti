using Atrasti.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atrasti.Data.Core
{
    public interface IProductRepository
    {
        Task<Product> FindProductByIdAsync(uint id);

        Task<ICollection<Product>> FindProductsByCompanyIdAsync(int companyId);

        Task CreateProductAsync(Product product);

        Task UpdateProductAsync(Product product);

        Task LikeProductAsync(ProductLike productLike);

        Task UnlikeProductAsync(ProductLike productLike);

        Task<ICollection<Product>> SearchByTags(string[] tags);
        
        Task<ICollection<Product>> SearchByMetaphoneTags(string[] metaPhoneSplit);

        Task<IList<Product>> SearchByTagsDistinct(string[] tags);

        Task<bool> RemoveProduct(uint id);
    }
}
