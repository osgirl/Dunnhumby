using System;
using System.Linq;

namespace Armin.Dunnhumby.Web.Helpers
{
    public static class CommonHelper
    {
        public static string GetRandomString(int len, bool upperChars, bool numbers)
        {
            string source = "abcdefghijklmnopqrstuvwxyz";
            if (upperChars)
                source += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (numbers)
                source += "0123456789";

            Random random = new Random();
            string randomString = "";
            for (int i = 0; i < len; i++)
                randomString += source[random.Next(source.Length)];

            return randomString;
        }

        /// <summary>
        /// Fetch and create paged result from a query
        /// </summary>
        /// <typeparam name="TModel">Type of data to fetch</typeparam>
        /// <param name="query">Linq query</param>
        /// <param name="pageNo">Number of page start with 1</param>
        /// <param name="pageSize">Number of items in a page</param>
        /// <returns>Fetched data with paging information</returns>
        public static PagedResult<TModel> ToPageOf<TModel>(this IQueryable<TModel> query, int pageNo = 1, int pageSize = 10)
        {
            var result = new PagedResult<TModel>()
            {
                PageSize = pageSize
            };

            result.GetPagedData(pageNo, query);
            return result;
        }

        /// <summary>
        /// Fetch and create paged result from a query
        /// </summary>
        /// <typeparam name="TInType">Type of data to fetch</typeparam>
        /// <typeparam name="TOutType">Output model type</typeparam>
        /// <param name="query">Linq query</param>
        /// <param name="setter">A function to convert in data type to out type</param>
        /// <param name="pageNo">Number of page start with 1</param>
        /// <param name="pageSize">Number of items in a page</param>
        /// <returns>Fetched data with paging information</returns>
        public static PagedResult<TInType, TOutType> ToPageOf<TInType, TOutType>(this IQueryable<TInType> query, Func<TInType, TOutType> setter, int pageNo = 1, int pageSize = 10)
        {
            var result = new PagedResult<TInType, TOutType>()
            {
                PageSize = pageSize
            };

            result.GetPagedData(pageNo, query, setter);
            return result;
        }
    }
}