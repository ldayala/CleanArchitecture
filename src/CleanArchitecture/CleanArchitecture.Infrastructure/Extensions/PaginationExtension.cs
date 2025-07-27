
using System.Linq.Expressions;

namespace CleanArchitecture.Infrastructure.Extensions
{
    //clase para extender la funcionalidad de un queryable

    public static class PaginationExtension
    {
        public static IQueryable<T> OrderByPropertyOrField<T>(
            this IQueryable<T> queryable,
            string propertyOrFieldName,
            bool ascending = true
        )
        {
            var elemetType = typeof(T);
            var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";

            var parameterExpression = Expression.Parameter(elemetType);
            var propertyOrFieldExpression = Expression.PropertyOrField(
                parameterExpression,
                propertyOrFieldName
            );

            var selector = Expression.Lambda(
                propertyOrFieldExpression,
                parameterExpression
            );

            var orderByExpression = Expression.Call(
                typeof(Queryable),
                orderByMethodName,
                new[] { elemetType, propertyOrFieldExpression.Type },
                queryable.Expression,
                selector
            );

            return queryable.Provider.CreateQuery<T>(orderByExpression);

        }
    }
}
