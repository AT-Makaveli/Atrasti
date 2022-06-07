using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;

namespace Atrasti.Data.Repository
{
    internal class BaseCategoriesRepository : BaseRepository, IBaseCategoriesRepository
    {
        public BaseCategoriesRepository(ConnectionProvider connectionFactory)
            : base(connectionFactory)
        {
        }

        public Task<IList<BaseCategory>> GetAllBaseCategories()
        {
            return WithConnection(connection =>
                connection.SelectMultipleAsync<BaseCategory>("SELECT * FROM BaseCategories;"));
        }

        public Task<IList<Product>> FindProductsByCategory(BaseCategory baseCategory)
        {
            return WithConnection(connection => connection.SelectMultipleAsync<Product>("SELECT * FROM products WHERE ProductCategory = @0;", baseCategory.Id));
        }

        public Task<IList<BaseCategory>> FindUserCategories(int companyId)
        {
            return WithConnection(connection => connection.SelectMultipleAsync<BaseCategory>(
                "SELECT b.* FROM BaseCategories b JOIN Categories c on b.Id = c.BaseCategoryId WHERE c.ProfileId = @0;",
                companyId));
        }
        
        public Task<BaseCategory> FindBaseCategoryById(int baseCategoryId)
        {
            return WithConnection(connection => connection.SelectSingleAsync<BaseCategory>(
                "SELECT * FROM BaseCategories WHERE Id = @0;",
                baseCategoryId));
        }

        public Task<int> RemoveUserCategories(int companyId, IEnumerable<int> toRemove)
        {
            return WithConnection(connection => connection.ExecuteAsync(
                "DELETE FROM Categories WHERE ProfileId = @profileId AND BaseCategoryId IN @categoryIds", new
                {
                    profileId = companyId,
                    categoryIds = toRemove.ToArray()
                }));
        }

        public Task<int> AddUserCategories(int companyId, IList<int> toAdd)
        {
            return WithConnection(async connection =>
            {
                await using DbTransaction transaction = await connection.BeginTransactionAsync();
                int rowsAffected = 0;
                foreach (int category in toAdd)
                {
                    rowsAffected += await transaction.Connection.Insert(
                        "INSERT INTO Categories(ProfileId, BaseCategoryId) VALUE (@0, @1)", companyId, category);
                }

                await transaction.CommitAsync();
                return rowsAffected;
            });
        }
        
        static SqlMapper.ICustomQueryParameter GetIntListTableValuedParameter(IEnumerable<int> ids)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            foreach (var id in ids)
            {
                dt.Rows.Add(id);
            }
            return dt.AsTableValuedParameter(" [dbo].[IntListTableType]");
        }
    }
}