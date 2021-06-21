using System;
using System.Linq;
using System.Linq.Expressions;

namespace PartyMaker.Common.Sorting
{
    public static class SortingBuilder
    {
        public static Expression<Func<T, object>> Build<T>(string sortField)
        {
            var targetTypeProps = typeof(T).GetProperties();
            var exprParam = Expression.Parameter(typeof(T), "x");
            var prop = targetTypeProps.FirstOrDefault(x => string.Equals(x.Name, sortField, StringComparison.InvariantCultureIgnoreCase));
            var propertyGetter = Expression.Property(exprParam, prop);

            return Expression.Lambda<Func<T, object>>(Expression.Convert(propertyGetter, typeof(object)), exprParam);
        }
    }
}
