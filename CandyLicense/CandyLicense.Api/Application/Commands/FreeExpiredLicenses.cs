using CandyLicense.Api.Data;
using CandyLicense.Api.Data.Entities;
using CandyLicense.Api.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CandyLicense.Api.Application.Commands
{
    public class FreeExpiredLicenses
    {
        public record Command() : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly CandyLicenseContext _context;
            private readonly ILogger<FreeExpiredLicenses> _logger;

            public Handler(CandyLicenseContext context, ILogger<FreeExpiredLicenses> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                // TODO: TEMP CODE TO INSERT TEST DATA. THIS DOESN'T BELONG HERE AT ALL
                if (await _context.Licenses.AnyAsync(cancellationToken) == false)
                {
                    await _context.Licenses.AddAsync(new License { Name = "Sega råttor" }, cancellationToken);
                    await _context.Licenses.AddAsync(new License { Name = "Chokladpraliner" }, cancellationToken);
                    await _context.Licenses.AddAsync(new License { Name = "Hallonbåtar" }, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                // END TEMP CODE:

                // TODO: Fix this DB query to work by not using extension method
                var licenses = (await _context.Licenses.ToListAsync(cancellationToken))
                    .Where(x => x.IsAvailableForRent() && x.Owner != null);

                foreach (var license in licenses)
                {
                    _logger.LogInformation("License {licenseName} is now available for rent", license.Name );
                    license.Expires = null;
                    license.Owner = null;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
