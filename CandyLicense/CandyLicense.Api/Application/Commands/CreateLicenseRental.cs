using CandyLicense.Api.Data;
using CandyLicense.Api.Extensions;
using CandyLicense.Api.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CandyLicense.Api.Application.Commands;

public class CreateLicenseRental
{
    public record Command(string UserId) : IRequest<CreateRentalResponse?>;

    public class Handler : IRequestHandler<Command, CreateRentalResponse?>
    {
        private readonly CandyLicenseContext _context;

        public Handler(CandyLicenseContext context)
        {
            _context = context;
        }

        public async Task<CreateRentalResponse?> Handle(Command request, CancellationToken cancellationToken)
        {
            // TODO: Fix this DB query
            var licenses = await _context.Licenses.ToListAsync(cancellationToken);

            var license = licenses.FirstOrDefault(x => x.IsAvailableForRent());

            if (license != null)
            {
                license.Owner = request.UserId;
                license.Expires = DateTime.Now.AddSeconds(15);   // TODO: No hardcoded values. Put in configuration! Also use UTC.
                await _context.SaveChangesAsync(cancellationToken);

                return new CreateRentalResponse { Name = license.Name };
            }

            return null;
        }
    }
}