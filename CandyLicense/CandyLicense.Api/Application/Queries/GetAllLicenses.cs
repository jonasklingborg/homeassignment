using CandyLicense.Api.Data;
using CandyLicense.Api.Data.Entities;
using CandyLicense.Api.Extensions;
using CandyLicense.Api.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CandyLicense.Api.Application.Queries;

public class GetAllLicenses
{
    public record Query() : IRequest<IEnumerable<GetLicenseResponse>>;

    public class Handler : IRequestHandler<Query, IEnumerable<GetLicenseResponse>>
    {
        private readonly CandyLicenseContext _context;

        public Handler(CandyLicenseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetLicenseResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Licenses.Select(x => new GetLicenseResponse
            {
                Name = x.Name,
                Status = x.IsAvailableForRent() ? "Available" : $"Owned by {x.Owner}. Expires in {x.SecondsLeft()} seconds"
            }).ToListAsync(cancellationToken);

            return result;
        }
    }
}