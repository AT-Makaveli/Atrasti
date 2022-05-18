using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atrasti.Data;
using Atrasti.Data.Core;
using Atrasti.Data.Models;
using Elasticsearch.Net;
using Phonix;

namespace Atrasti.Utils
{
    public class SearchAlgorithm
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private static readonly DoubleMetaphone DoubleMetaphone = new DoubleMetaphone(6);

        public SearchAlgorithm(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<ICollection<SearchResult>> SearchAsync(string searchQuery)
        {
            ICollection<Product> products = await SearchProductsAsync(searchQuery);
            ICollection<AtrastiUser> users = await SearchCompanies(searchQuery);

            ICollection<SearchResult> results = new List<SearchResult>();
            foreach (Product product in products)
            {
                results.Add(new SearchResult()
                {
                    Title = product.Title
                });
            }

            foreach (AtrastiUser user in users)
            {
                results.Add(new SearchResult()
                {
                    Title = user.Company
                });
            }

            return results;
        }
        
        public async Task<int[]> SearchCompanyIdsAsync(string searchQuery)
        {
            ICollection<Product> products = await SearchProductsAsync(searchQuery);
            ICollection<AtrastiUser> users = await SearchCompanies(searchQuery);

            IList<int> results = new List<int>();
            foreach (Product product in products)
            {
                if (!results.Contains(product.CompanyId)) results.Add(product.CompanyId);
            }

            foreach (AtrastiUser user in users)
            {
                if (!results.Contains(user.Id)) results.Add(user.Id);
            }

            return results.ToArray();
        }

        private Task<ICollection<AtrastiUser>> SearchCompanies(string searchQuery)
        {
            string[] querySplit = searchQuery.Split(' ');
            IList<string> searchEntry = new List<string>();
            for (int i = 0; i < querySplit.Length; i++)
            {
                string queryEntry = querySplit[i];
                if (queryEntry == "" || queryEntry.Length < 3) continue;

                searchEntry.Add(queryEntry);
            }

            return _userRepository.SearchUsersByCompanyAsync(searchEntry.ToArray());
        }

        private Task<ICollection<Product>> SearchProductsAsync(string searchQuery)
        {
            string[] querySplit = searchQuery.Split(' ');
            IList<string> metaphoneKeys = new List<string>();

            for (int i = 0; i < querySplit.Length; i++)
            {
                string queryEntry = querySplit[i];
                if (queryEntry == "") continue;

                metaphoneKeys.Add(DoubleMetaphone.BuildKey(queryEntry));
            }

            return _productRepository.SearchByMetaphoneTags(metaphoneKeys.ToArray());
        }

        public class SearchResult
        {
            public string Title { get; set; }
        }
    }
}