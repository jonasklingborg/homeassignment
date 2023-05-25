using CandyLicense.Api.Application.Commands;
using CandyLicense.Api.Data;
using CandyLicense.Api.Extensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CandyLicense.Api.Validation;

public class CreateLicenseRentalValidator : AbstractValidator<CreateLicenseRental.Command>
{
    private readonly CandyLicenseContext _context;

    public CreateLicenseRentalValidator(CandyLicenseContext context)
    {
        _context = context;
        RuleFor(x => x.UserId)
            .MustAsync(async (userId, c) => await DoesNotHaveLicense(userId, c))
            .WithMessage("This user already have a license");
    }

    private async Task<bool> DoesNotHaveLicense(string userId, CancellationToken cancellationToken)
    {
        // TODO: Fix this DB query
        var licenses = await _context.Licenses.ToListAsync(cancellationToken);
        var rented = licenses.FirstOrDefault(x => !x.IsAvailableForRent() && x.Owner == userId);

        return rented == null;
    }
}