using CandyLicense.Api.Data;
using CandyLicense.Api.Data.Entities;
using FluentValidation;
using MediatR;

namespace CandyLicense.Api.Application.Commands
{
    public class AddLicense
    {
        public record Command(string LicenseName) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly CandyLicenseContext _context;
            private readonly IValidator<Command> _validator;

            public Handler(CandyLicenseContext context, IValidator<Command> validator)
            {
                _context = context;
                _validator = validator;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                // TODO: Validator could be set up to be run by the MediatR framework instead
                await _validator.ValidateAndThrowAsync(request, cancellationToken);

                await _context.AddAsync(new License { Name = request.LicenseName }, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
