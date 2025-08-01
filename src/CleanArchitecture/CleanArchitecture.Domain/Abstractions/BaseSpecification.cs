﻿
using System.Linq.Expressions;

namespace CleanArchitecture.Domain.Abstractions
{
    public abstract class BaseSpecification<TEntity, TEntityId>
        : ISpecification<TEntity, TEntityId>
        where TEntity : Entity<TEntityId>
        where TEntityId : class
    {
        protected BaseSpecification()
        {
        }
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<TEntity, bool>>? Criteria { get; }
        public List<Expression<Func<TEntity, object>>> Includes =>[];

        public Expression<Func<TEntity, object>>? OrderBy {get;private set;}

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        public Expression<Func<TEntity, object>>? GroupBy { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void AddGroupBy(Expression<Func<TEntity, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
        }
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

    }
}
