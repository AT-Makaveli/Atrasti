using System.Collections.Generic;
using Atrasti.API.Models.Search;
using Atrasti.Data.Models;

namespace Atrasti.API.Helpers
{
    public static class SearchHelpers
    {
        public static IList<SearchEntry_Res> MapSearchEntries(this IEnumerable<AtrastiUser> atrastiUsers)
        {
            IList<SearchEntry_Res> results = new List<SearchEntry_Res>();

            foreach (AtrastiUser atrastiUser in atrastiUsers)
            {
                results.Add(atrastiUser.MapSearchEntry());
            }
            
            return results;
        }

        public static SearchEntry_Res MapSearchEntry(this AtrastiUser atrastiUser)
        {
            return new SearchEntry_Res()
            {
                CompanyId = atrastiUser.Id,
                CompanyName = atrastiUser.Company,
                Logo = atrastiUser.CompanyLogo
            };
        }
    }
}