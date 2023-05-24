using CandyLicense.Api.Requests;
using CandyLicense.Api.Responses;

namespace CandyLicense.Api.Endpoints
{
    public class LicenseEndpoint
    {
        public static void SetupRoute(IEndpointRouteBuilder app)
        {
            app.MapPost("api/license", AddLicense)
                .WithName("AddLicense");

            app.MapGet("api/license", GetLicenses)
                .WithName("GetLicenses");

            app.MapPost("api/rental", CreateRental)
                .WithName("RentLicense");

            app.MapGet("api/rental", GetRental)
                .WithName("GetRental");
        }

        private static List<License> Licenses = new()
        {
            new() { Name = "Gelehallon" },
            new() { Name = "Sega råttor" },
        };

        private static async Task<IResult> AddLicense(AddLicenseRequest request)
        {
            Licenses.Add(new License() { Name = request.Name });

            //return Results.Conflict();

            //return Results.Created();

            return Results.Ok();
        }

        private static async Task<IResult> GetLicenses()
        {
            var result = Licenses.Select(x => new GetLicenseResponse
            {
                Name = x.Name,
                Status = x.IsAvailableForRent ? "Available" : $"Owned by {x.Owner}. Expires in {x.SecondsLeft} seconds",
            });

            return Results.Ok(result);
        }

        private static async Task<IResult> CreateRental() 
        {
            // TODO: Fix real login and authentication
            //var user = httpContext.User.Identity?.Name;
            var user = "Jonas Klingborg";

            var license = Licenses.FirstOrDefault(x => x.IsAvailableForRent);

            if (license != null)
            {
                license.Owner = user;
                license.Expires = DateTime.Now.AddSeconds(15);   // TODO: No hardcoded values. Put in configuration!
                return Results.Ok(new CreateRentalResponse {Name = license.Name});
            }

            return Results.NotFound();
        }

        private static async Task<IResult> GetRental()
        {
            // TODO: Fix real login and authentication
            //var user = httpContext.User.Identity?.Name;
            var user = "Jonas Klingborg";

            var license = Licenses.FirstOrDefault(x => x.Owner == user && !x.IsAvailableForRent);

            return license != null ? Results.Ok(new GetRentalResponse { Name = license.Name }) : Results.NotFound();
        }
    }

    public class License
    {
        public string Name { get; set; } = string.Empty;
        public string? Owner { get; set; }
        public DateTime? Expires { get; set; }

        public bool IsAvailableForRent => Expires == null || Expires.Value < DateTime.Now;
        public int SecondsLeft => IsAvailableForRent ? 0 : (Expires.Value - DateTime.Now).Seconds;
    }


}
