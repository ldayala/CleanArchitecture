using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Paginations;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Users.GetUserPagination
{
    internal sealed class GetUserPaginationQueryHandler(IPaginationRepository<User, UserId> paginationRepository) : IQueryHandler<GetUserPaginationQuery, PagedResults<User, UserId>>
    {
        private readonly IPaginationRepository<User, UserId> _paginationRepository = paginationRepository;
        public async Task<Result<PagedResults<User, UserId>>> Handle(GetUserPaginationQuery request, CancellationToken cancellationToken)
        {
            var predicateB = PredicateBuilder.New<User>(true);

            if (!string.IsNullOrEmpty(request.Search))
            {
                predicateB = predicateB.Or(x => x.Nombre == new Nombre(request.Search));
                predicateB = predicateB.Or(x => x.Email == new Email(request.Search));
            }

            return await _paginationRepository.GetPaginationAsync(
                    predicateB,
                    x => x.Include(x => x.Roles!).ThenInclude(y => y.Permissions!),
                    request.PageNumber,
                    request.PageSize,
                    request.OrderBy!,
                    request.OrderAsc
               );

        }
    }
}
