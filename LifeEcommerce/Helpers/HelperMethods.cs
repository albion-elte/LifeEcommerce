using System.Linq.Expressions;

namespace LifeEcommerce.Helpers
{
    public static class HelperMethods
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool searchCondition, Expression<Func<T, bool>> predicate)
        {
            //if(searchCondition)
            //{
            //    return query.Where(predicate);
            //}
            //else
            //{
            //    return query;
            //}

            return searchCondition ? query.Where(predicate) : query;
        }

    }
}
