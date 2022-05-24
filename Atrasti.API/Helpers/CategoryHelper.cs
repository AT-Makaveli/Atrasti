using System.Collections.Generic;
using Atrasti.API.Models.Categories;
using Atrasti.Data.Models;
using Nest;

namespace Atrasti.API.Helpers
{
    public static class CategoryHelper
    {
        public static IList<BaseCategory_Res> MapCategoriesRes(this IEnumerable<BaseCategory> categories)
        {
            IList<BaseCategory_Res> result = new List<BaseCategory_Res>();
            foreach (BaseCategory baseCategory in categories)
            {
                result.Add(baseCategory.MapCategoryRes());
            }

            return result;
        }

        public static BaseCategory_Res MapCategoryRes(this BaseCategory baseCategory)
        {
            return new BaseCategory_Res
            {
                Id = baseCategory.Id,
                Title = baseCategory.Title
            };
        }
    }
}