using System.Collections.Generic;
using System.Threading.Tasks;
using Atrasti.Data.Models;

namespace Atrasti.Data.Core
{
    public interface IBaseCategoriesRepository
    {
        Task<IList<Product>> FindProductsByCategory(BaseCategory baseCategory);
        
        Task<IList<BaseCategory>> GetAllBaseCategories();

        Task<IList<BaseCategory>> FindUserCategories(int companyId);

        Task<int> RemoveUserCategories(int companyId, IList<int> toRemove);
        
        Task<int> AddUserCategories(int companyId, IList<int> toAdd);
    }
}