namespace CleanArchitecture.Domain.Abstractions
{
    public class PagedResults<TEntity,TEntityId>
        where TEntity : Entity<TEntityId>
        where TEntityId : class
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalNumberOfpages { get; set; }
        public int TotalNumberOfRecords { get; set; }
        public IReadOnlyList<TEntity> Results { get; set; } = new List<TEntity>();
    }
}

