using CandyLicense.Api.Data;
using CandyLicense.Api.Extensions;
using CandyLicense.Api.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CandyLicense.Api.Application.Commands;

public class CreateLicenseRental
{
    public record Command(string UserId) : IRequest<CreateRentalResponse?>;

    public class Handler : IRequestHandler<Command, CreateRentalResponse?>
    {
        private readonly CandyLicenseContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(CandyLicenseContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<CreateRentalResponse?> Handle(Command request, CancellationToken cancellationToken)
        {
            // TODO: Validator could be set up to be run by the MediatR framework instead
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            // TODO: Fix this DB query to work by not using extension method
            var licenses = (await _context.Licenses.ToListAsync(cancellationToken)).Where(x => x.IsAvailableForRent()).ToList();

            if (!licenses.Any())
            {
                return null;
            }

            var index = new Random().Next(0, licenses.Count - 1);

            var license = licenses[index];

            license.Owner = request.UserId;
            license.Expires = DateTime.Now.AddSeconds(15);   // TODO: No hardcoded values. Put in configuration! Also use UTC.
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateRentalResponse { Name = license.Name };
        }
    }
}