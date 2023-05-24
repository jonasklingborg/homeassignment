using CandyLicense.Api.Data;
using CandyLicense.Api.Data.Entities;
using MediatR;

namespace CandyLicense.Api.Application.Commands
{
    public class AddLicense
    {
        public record Command(string licenseName) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly CandyLicenseContext _context;

            public Handler(CandyLicenseContext context)
            {
                _context = context;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                await _context.AddAsync(new License { Name = request.licenseName }, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
