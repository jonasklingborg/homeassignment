using CandyLicense.Web.Services.LicenseApiModels;

namespace CandyLicense.Web.Services;

public interface ILicenseApi
{
    Task<List<GetLicenseResponse>> GetLicensesAsync();
    Task AddLicenseAsync(string name);
    Task<string?> RentLicenseAsync();
    Task<string?> GetRentalAsync();
}