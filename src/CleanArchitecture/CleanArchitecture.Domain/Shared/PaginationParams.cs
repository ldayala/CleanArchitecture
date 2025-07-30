
namespace CleanArchitecture.Domain.Shared
{
    public record PaginationParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; init; } = 1;
        private int _pageSize = 10;
        public int PageSize { 
            get=> _pageSize;
            set => _pageSize = (value >MaxPageSize)? MaxPageSize : value;

        }

        public string ? OrderBy { get; set; }
        public bool OrderAsc { get; set; } = true;

        public string? Search { get; set; }
    }
}
