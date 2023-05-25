using CandyLicense.Api.Application.Commands;
using CandyLicense.Api.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CandyLicense.Api.Validation
{
    public class AddLicenseValidator : AbstractValidator<AddLicense.Command>
    {
        private readonly CandyLicenseContext _context;

        public AddLicenseValidator(CandyLicenseContext context)
        {
            _context = context;
            RuleFor(x => x.LicenseName)
                .MustAsync(async (licenseName, c) => await IsNonExisting(licenseName))
                .WithMessage("This license name already exists");
        }

        private async Task<bool> IsNonExisting(string licenseName)
        {
            var license = await _context.Licenses.FirstOrDefaultAsync(x => x.Name == licenseName);
            return license == null;
        }
    }
}
