using CandyLicense.Api.Data;
using CandyLicense.Api.Extensions;
using CandyLicense.Api.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CandyLicense.Api.Application.Queries;

public class GetRental
{
    public record Query(string UserId) : IRequest<GetRentalResponse?>;

    public class Handler : IRequestHandler<Query, GetRentalResponse?>
    {
        private readonly CandyLicenseContext _context;

        public Handler(CandyLicenseContext context)
        {
            _context = context;
        }

        public async Task<GetRentalResponse?> Handle(Query request, CancellationToken cancellationToken)
        {
            // TODO: Fix this DB query
            var licenses = await _context.Licenses.ToListAsync(cancellationToken);

            var license = licenses
                .Where(x => x.Owner == request.UserId && !x.IsAvailableForRent())
                .Select(x =>
                    new GetRentalResponse
                    {
                        Name = x.Name
                    }).FirstOrDefault();

            return license;
        }
    }
}