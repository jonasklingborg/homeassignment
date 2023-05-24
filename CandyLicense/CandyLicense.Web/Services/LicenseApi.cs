using System.Net;

namespace CandyLicense.Web.Services
{
    public interface ILicenseApi
    {
        Task<List<GetLicenseResponse>> GetLicensesAsync();
        Task AddLicenseAsync(string name);
        Task<string?> RentLicenseAsync();
        Task<string?> GetRentalAsync();
    }
    
    public class LicenseApi : ILicenseApi
    {
        private readonly string _baseAddress;

        public LicenseApi()
        {
            _baseAddress = "https://localhost:7117/api";
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

            var result = await client.PostAsync(_baseAddress + "/license", JsonContent.Create(new AddLicenseRequest { Name = name}));

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
                return (await result.Content.ReadFromJsonAsync<CreateRentalResponse>()).Name;
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
                return (await result.Content.ReadFromJsonAsync<GetRentalResponse>()).Name;
            }


            throw new Exception("Error getting rental");
        }
    }

    public class GetLicenseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class AddLicenseRequest
    {
        public string? Name { get; set; }
    }

    public class CreateRentalResponse
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GetRentalResponse
    {
        public string Name { get; set; } = string.Empty;
    }
}
