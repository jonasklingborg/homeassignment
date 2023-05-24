using CandyLicense.Api.Data.Entities;

namespace CandyLicense.Api.Extensions
{
    public static class LicenseExtensions
    {
        public static bool IsAvailableForRent(this License license) => license.Expires == null || license.Expires.Value < DateTime.Now;
        public static int SecondsLeft(this License license) => license.IsAvailableForRent() ? 0 : (license.Expires.Value - DateTime.Now).Seconds;
    }
}
