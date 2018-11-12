using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Colin.FilterCore
{
    public static class QueryExtensions
    {
        public static IQueryable<T> DisableFilter<T>(this IQueryable<T> query, string filtername, params string[] filternames) where T : class
        {
            if (string.IsNullOrWhiteSpace(filtername))
            {
                throw new ArgumentException("message", nameof(filtername));
            }

            var disableFilters = filternames.Concat(new[] { filtername }).ToArray();

            var exp = query.BuildExpression(disableFilters);
            var result = query.IgnoreQueryFilters();
            if (exp != null)
            {
                result = result.Where(exp);
            }
            return result;
        }


        public static IQueryable<T> EnableFilter<T>(this IQueryable<T> query, string filtername, params string[] filternames) where T : class
        {
            if (string.IsNullOrWhiteSpace(filtername))
            {
                throw new ArgumentException("message", nameof(filtername));
            }

            var disableFilters = filternames.Concat(new[] { filtername }).ToArray();

            var exp = query.BuildExpression(disableFilters, true);
            var result = query.IgnoreQueryFilters();
            if (exp != null)
            {
                result = result.Where(exp);
            }
            return result;
        }

        public static Dictionary<string, FilterInfo> GetFilters<T>(this IQueryable<T> query) where T : class
        {
            return
              GlobaQuerylFilters
                .Filters
                .Where(x => x.Value.FilterType.IsAssignableFrom(typeof(T)))
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
