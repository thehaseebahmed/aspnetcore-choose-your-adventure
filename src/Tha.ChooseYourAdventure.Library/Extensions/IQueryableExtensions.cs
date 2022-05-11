using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using Tha.ChooseYourAdventure.Library.Core;

namespace Tha.ChooseYourAdventure.Library.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, string filter)
        {
            if (queryable == null) { throw new ArgumentNullException("queryable"); }
            if (string.IsNullOrEmpty(filter.Trim())) { throw new ArgumentNullException("filter"); }

            var containsRegex = new Regex(@"contains\((?<Property>(.*)),[ ]?(?<Value>(.*))\)");
            if (containsRegex.IsMatch(filter))
            {
                var matches = containsRegex.Match(filter);
                var property = matches.Groups["Property"];
                var value = matches.Groups["Value"];
                filter = containsRegex.Replace(
                    filter,
                    $"{property}.contains({value})"
                    );
            }

            queryable = queryable.Where(filter);
            return queryable;
        }

        public static IQueryable<T> Order<T>(this IQueryable<T> queryable, string order)
        {
            if (queryable == null) { throw new ArgumentNullException("queryable"); }
            if (string.IsNullOrEmpty(order)) { throw new ArgumentNullException("order"); }

            queryable = queryable.OrderBy(order);
            return queryable;
        }

        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, IPagedGetRequest request, out int count)
            where T : class
        {
            if (queryable == null) { throw new ArgumentNullException("queryable"); }

            count = 0;
            if (request.Count)
            {
                count = queryable.Count();
            }

            if (request.Skip > 0)
            {
                queryable = queryable.Skip(request.Skip);
            }

            if (request.Limit > 0)
            {
                queryable = queryable.Take(request.Limit);
            }

            return queryable;
        }
    }
}
