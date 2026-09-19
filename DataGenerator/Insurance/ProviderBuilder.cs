using Bogus;
using DataGenerator.Insurance.Models;

namespace DataGenerator.Insurance;

internal class ProviderBuilder
{
    // Static header definition for CSV file export mapping
    internal static readonly string[] Headers =
    [
        "Service Provider ID", "Service Provider Name", "Provider Address Line 1", "Provider Address Line 2",
        "Provider City", "Provider Zip code", "Provider State", "Provider Postal code", "Provider Country",
        "Provider Email", "Provider Phone"
    ];

    internal static List<Provider> GenerateData(int count)
    {
        return GetGenerator()
            .Generate(count);
    }

    private static Faker<Provider> GetGenerator()
    {
        return new Faker<Provider>("en")
            .RuleFor(p => p.ServiceProviderId, f => $"sp{f.IndexFaker + 1:D8}")
            .RuleFor(p => p.ServiceProviderName, f => f.Company.CompanyName())
            .RuleFor(p => p.ProviderAddressLine1, f => f.Address.StreetAddress())
            .RuleFor(p => p.ProviderAddressLine2, f => f.Address.SecondaryAddress())
            .RuleFor(p => p.ProviderCity, f => f.Address.City())
            .RuleFor(p => p.ProviderZipCode, f => f.Address.ZipCode())
            .RuleFor(p => p.ProviderState, f => f.Address.StateAbbr())
            .RuleFor(p => p.ProviderPostalCode, f => f.Address.ZipCode())
            .RuleFor(p => p.ProviderCountry, "USA")
            .RuleFor(p => p.ProviderEmail, f => f.Random.Bool(0.15f) ? "" : f.Internet.Email())
            .RuleFor(p => p.ProviderPhone, f => f.Random.Bool(0.1f) ? "" : f.Phone.PhoneNumber("###-###-####"));
    }
}