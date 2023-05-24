using System.Net;
using CandyLicense.Web.Services.LicenseApiModels;

namespace CandyLicense.Web.Services
{
    public class LicenseApi : ILicenseApi
    {
        private readonly string _baseAddress;

        public LicenseApi(AppSettings appSettings)
        {
            _baseAddress = appSettings.LicenseApiBaseAddress;
        }

        public async Task<List<GetLicenseResponse>> GetLicensesAsync()
        {
            using var client = new HttpClient();

            var result = await client.GetAsync(_baseAddress + "/license");

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<List<GetLicenseResponse>>() ?? new List<GetLicenseResponse>();
            }

            throw new Exception("Error fetching licenses");
        }

        public async Task AddLicenseAsync(string name)
        {
            using var client = new HttpClient();

            var result = await client.PostAsync(_baseAddress + "/license", JsonContent.Create(new AddLicenseRequest { Name = name }));

            if (!result.IsSuccessStatusCode)
                throw new Exception("Error adding license");
        }

        public async Task<string?> RentLicenseAsync()
        {
            using var client = new HttpClient();

            var result = await client.PostAsync(_baseAddress + "/rental", new StringContent(string.Empty));

            if (result.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (result.IsSuccessStatusCode)
            {
                return (await result.Content.ReadFromJsonAsync<CreateRentalResponse>())?.Name;
            }

            throw new Exception("Error creating rental");
        }

        public async Task<string?> GetRentalAsync()
        {
            using var client = new HttpClient();

            var result = await client.GetAsync(_baseAddress + "/rental");

            if (result.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (result.IsSuccessStatusCode)
            {
                return (await result.Content.ReadFromJsonAsync<GetRentalResponse>())?.Name;
            }


            throw new Exception("Error getting rental");
        }
    }
}
