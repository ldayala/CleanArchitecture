using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Domain.Vehiculos.Specifications;

namespace CleanArchitecture.Application.Vehiculos.GetVehiculosByPagination
{
    internal sealed class GetVehiculosByPaginationQueryHandler(IVehiculoRepository vehiculoRepository)
        : IQueryHandler<GetVehiculosByPaginationQuery, PaginationResult<Vehiculo, VehiculoId>>
    {
        private readonly IVehiculoRepository _vehiculoRepository = vehiculoRepository;
        public async Task<Result<PaginationResult<Vehiculo, VehiculoId>>> Handle(GetVehiculosByPaginationQuery request, CancellationToken cancellationToken)
        {
            var spec = new VehiculoPaginationSpecification(request.Short!, request.PageIndex, request.PageSize, request.Modelo!);
            var specCount = new VehiculoPaginationCountingSpecification(request.Modelo!);
            var records = await _vehiculoRepository.GetAllWithSpec(spec);
            var totalCount = await _vehiculoRepository.CountAsync(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalCount) / Convert.ToDecimal(request.PageSize)); //Math.ceil is used to round up the total pages for example 9.1 = 10 pages
            int totalPages = Convert.ToInt32(rounded);
            var recordByPage = records.Count();

            return new PaginationResult<Vehiculo, VehiculoId>
            {
                Data = records.ToList(),
                Count = totalCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                PageCount = totalPages,
                ResultByPage = recordByPage
            };
        }
    }
}
