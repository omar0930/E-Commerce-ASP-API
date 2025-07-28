using System.Linq.Expressions;

namespace Store.Repository.Specifications
{
    public interface ISpecification<T>
    {
        // Criteria
        Expression<Func<T, bool>> Criteria { get; }
        // Includes
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>> OrderByAsc { get; }
        Expression<Func<T, object>> OrderByDesc { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; }
    }
}
