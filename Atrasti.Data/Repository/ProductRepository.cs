using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Atrasti.Data.Repository
{
    internal class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(ConnectionProvider connectionFactory) : base(connectionFactory)
        {
        }

        public Task CreateProductAsync(Product product)
        {
            return WithConnection(async connection =>
            {
                uint id = await connection.ExecuteScalarAsync<uint>("INSERT INTO `products`(`CompanyId`, `Title`, `Description`, `Tags`, `PhoneticTags`, `ProductCategory`) VALUES (@companyId, @title, @desc, @tags, @phoneticTags, @productCategory);SELECT LAST_INSERT_ID();", new
                {
                    companyId = product.CompanyId,
                    title = product.Title,
                    desc = product.Description,
                    tags = JsonConvert.SerializeObject(product.Tags),
                    phoneticTags = string.Join(' ', product.PhoneticTags),
                    productCategory = product.ProductCategory
                });
                product.Id = id;
            }, CancellationToken.None);
        }

        public async Task<bool> RemoveProduct(uint id)
        {
            int affectedRows = await WithConnection(connection => connection.Insert("DELETE FROM products WHERE id = @0", id));

            return affectedRows == 1;
        }
        
        public Task<Product> FindProductByIdAsync(uint id)
        {
            return WithConnection(async connection =>
            {
                Product product = null;
                using (var reader = await connection.ExecuteReaderAsync(
                    "SELECT products.*, product_likes.* FROM products LEFT JOIN product_likes ON products.Id = product_likes.RefId WHERE products.Id = @id",
                    new {id}))
                {
                    while (await reader.ReadAsync())
                    {
                        if (product == null)
                        {
                            product = new Product();
                            product.Serialize(reader);
                            product.SerializeLikes(reader);
                        }
                        else
                        {
                            product.SerializeLikes(reader);
                        }
                    }
                }

                return product;
            }, CancellationToken.None);
        }

        public Task<ICollection<Product>> FindProductsByCompanyIdAsync(int companyId)
        {
            return WithConnection(async connection =>
            {
                IDictionary<uint, Product> products = new Dictionary<uint, Product>();
                using (var reader = await connection.ExecuteReaderAsync(
                    "SELECT products.*, product_likes.* FROM products LEFT JOIN product_likes ON products.Id = product_likes.RefId WHERE products.CompanyId = @id",
                    new {id = companyId}))
                {
                    while (await reader.ReadAsync())
                    {
                        uint id = reader.GetData<uint>("id");
                        if (products.TryGetValue(id, out Product product))
                        {
                            product.SerializeLikes(reader);
                        }
                        else
                        {
                            product = new Product();
                            product.Serialize(reader);
                            product.SerializeLikes(reader);
                            products.Add(product.Id, product);
                        }
                    }
                }

                return products.Values;
            }, CancellationToken.None);
        }

        public Task LikeProductAsync(ProductLike productLike)
        {
            return WithConnection(connection =>
            {
                return connection.ExecuteAsync(
                    "INSERT INTO `product_likes`(`RefId`, `UserId`) VALUES (@refId, @userId);", new
                    {
                        refId = productLike.RefId,
                        userId = productLike.UserId
                    });
            }, CancellationToken.None);
        }

        public Task<ICollection<Product>> SearchByTags(string[] tags)
        {
            StringBuilder queryBuilder = new StringBuilder();
            int index = 0;
            IList<string> parameters = new List<string>();
            for (int i = 0; i < tags.Length; i++)
            {
                string tag = tags[i];
                if (string.IsNullOrEmpty(tag))
                {
                    continue;
                }

                if (index == 0)
                {
                    queryBuilder.Append("SELECT * FROM products WHERE JSON_SEARCH(LOWER(Tags), 'all', @" + index++ +
                                        ") IS NOT NULL OR ");
                    parameters.Add(string.Format("%{0}%", tag.ToLower()));
                }
                else
                {
                    queryBuilder.Append("JSON_SEARCH(LOWER(Tags), 'all', @" + index++ + ") IS NOT NULL OR ");
                    parameters.Add(string.Format("%{0}%", tag.ToLower()));
                }
            }

            string query = queryBuilder.ToString();
            query = query.Substring(0, query.Length - 3);
            return WithConnection(async connection =>
            {
                ICollection<Product> prod = await connection.SelectMultipleAsync<Product>(query, parameters.ToArray());
                return prod;
            }, CancellationToken.None);
        }

        public async Task<ICollection<Product>> SearchByMetaphoneTags(string[] metaPhoneSplit)
        {
            if (metaPhoneSplit.Length == 0) return new List<Product>();

            IList<string> parameters = new List<string>();
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT * FROM products WHERE (");

            for (int i = 0; i < metaPhoneSplit.Length; i++)
            {
                string metaPhone = metaPhoneSplit[i];
                if (i == 0)
                    queryBuilder.Append("PhoneticTags LIKE @" + i);
                else
                    queryBuilder.Append(" AND PhoneticTags LIKE @" + i);
                parameters.Add(string.Format("%{0}%", metaPhone.ToLower()));
            }

            queryBuilder.Append(");");

            return await WithConnection(async connection =>
            {
                ICollection<Product> products =
                    await connection.SelectMultipleAsync<Product>(queryBuilder.ToString(), parameters.ToArray());

                return products;
            }, CancellationToken.None);
        }

        public async Task<IList<Product>> SearchByTagsDistinct(string[] tags)
        {
            if (tags.Length == 0 || string.IsNullOrEmpty(tags[0])) return new List<Product>();

            StringBuilder queryBuilder = new StringBuilder();
            int index = 0;
            IList<string> parameters = new List<string>();
            for (int i = 0; i < tags.Length; i++)
            {
                string tag = tags[i];
                if (string.IsNullOrEmpty(tag))
                {
                    continue;
                }
                else if (index == 0)
                {
                    queryBuilder.Append("SELECT * FROM products WHERE JSON_SEARCH(LOWER(Tags), 'all', @" + index++ +
                                        ") IS NOT NULL OR ");
                    parameters.Add(string.Format("%{0}%", tag.ToLower()));
                }
                else
                {
                    queryBuilder.Append("JSON_SEARCH(LOWER(Tags), 'all', @" + index++ + ") IS NOT NULL OR ");
                    parameters.Add(string.Format("%{0}%", tag.ToLower()));
                }
            }

            string query = queryBuilder.ToString();
            query = query.Substring(0, query.Length - 3);
            query += " GROUP BY CompanyId;";
            return await WithConnection(
                connection => { return connection.SelectMultipleAsync<Product>(query, parameters.ToArray()); },
                CancellationToken.None);
        }

        public Task UnlikeProductAsync(ProductLike productLike)
        {
            return WithConnection(connection =>
            {
                return connection.ExecuteAsync(
                    "DELETE FROM product_likes WHERE `RefId` = @refId AND `UserId` = @userId LIMIT 1;", new
                    {
                        refId = productLike.RefId,
                        userId = productLike.UserId
                    });
            }, CancellationToken.None);
        }

        public Task UpdateProductAsync(Product Product)
        {
            throw new System.NotImplementedException();
        }
    }
}