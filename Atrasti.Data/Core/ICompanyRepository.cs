using Atrasti.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atrasti.Data.Core
{
    public interface ICompanyRepository
    {
        Task<Company> FindCompanyByIdAsync(int id);

        Task CreateCompanyAsync(Company company);

        Task UpdateCompanyAsync(Company company);

        Task<ICollection<Company>> Get20ProductsAsync();
    }
}
