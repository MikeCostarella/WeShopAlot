using WeShopAlot.Data.Models.Base;
using WeShopAlot.Data.Specifications.Base;
using Microsoft.EntityFrameworkCore;

namespace WeShopAlot.Data.Utilities.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : BasePersistentObject
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var queryable = inputQuery;

            if (specification.Criteria != null)
            {
                queryable = queryable.Where(specification.Criteria);
            }

            if (specification.OrderBy != null)
            {
                queryable = queryable.OrderBy(specification.OrderBy);
            }

            if (specification.OrderByDescending != null)
            {
                queryable = queryable.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPagingEnabled)
            {
                queryable = queryable.Skip(specification.Skip).Take(specification.Take);
            }

            queryable = specification.Includes.Aggregate(queryable, (current, include) => current.Include(include));
            queryable = specification.IncludeStrings.Aggregate(queryable, (current, include) => current.Include(include));

            return queryable;
        }
    }
}
