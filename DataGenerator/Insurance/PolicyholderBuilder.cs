using Bogus;
using DataGenerator.Insurance.Models;

namespace DataGenerator.Insurance;

internal class PolicyholderBuilder
{
    internal static readonly string[] Headers = 
    [
        "Policyholder ID", "Policyholder First Name", "Policyholder Last Name", "Policyholder Email",
        "Policyholder Phone Number", "Policyholder Phone Type", "Policyholder Mailing Address Line 1",
        "Policyholder Mailing Address Line 2", "Policyholder Mailing City", "Policyholder Mailing State / County",
        "Policyholder Mailing Zip Code", "Policyholder Mailing Postal code", "Policyholder Mailing Country",
        "Policyholder Billing Address Line 1", "Policyholder Billing Address Line 2", "Policyholder Billing City",
        "Policyholder Billing State / County", "Policyholder Billing Zip Code", "Policyholder Billing Postal code",
        "Policyholder Billing Country", "Consent type"
    ];


    internal static List<Policyholder> GenerateData(int count)
    {
        return GetGenerator()
            .Generate(count);
    }

    private static Faker<Policyholder> GetGenerator()
    {
        return new Faker<Policyholder>()
            .RuleFor(p => p.PolicyholderId, f => $"ph{f.IndexFaker + 1:D8}")
            .RuleFor(p => p.PolicyholderFirstName, f => f.Name.FirstName())
            .RuleFor(p => p.PolicyholderLastName, f => f.Name.LastName())
            .RuleFor(p => p.PolicyholderEmail, (f, p) => f.Internet.Email(p.PolicyholderFirstName, p.PolicyholderLastName))
            .RuleFor(p => p.PolicyholderPhoneNumber, f => f.Random.Bool(0.15f) ? "" : f.Random.Replace("555#######"))
            .RuleFor(p => p.PolicyholderPhoneType, f => f.Phone.PhoneNumber("###-###-####"))
            .RuleFor(p => p.PolicyholderMailingAddressLine1, f => f.Address.StreetAddress())
            .RuleFor(p => p.PolicyholderMailingAddressLine2, f => f.Address.SecondaryAddress())
            .RuleFor(p => p.PolicyholderMailingCity, f => f.Address.City())
            .RuleFor(p => p.PolicyholderMailingStateCounty, f => f.Address.StateAbbr())
            .RuleFor(p => p.PolicyholderMailingZipCode, f => f.Address.ZipCode())
            .RuleFor(p => p.PolicyholderMailingPostalCode, (_, p) => p.PolicyholderMailingZipCode)
            .RuleFor(p => p.PolicyholderMailingCountry, "USA")
            .RuleFor(p => p.PolicyholderBillingAddressLine1, f => f.Address.StreetAddress())
            .RuleFor(p => p.PolicyholderBillingAddressLine2, f => f.Address.SecondaryAddress())
            .RuleFor(p => p.PolicyholderBillingCity, f => f.Address.City())
            .RuleFor(p => p.PolicyholderBillingStateCounty, f => f.Address.StateAbbr())
            .RuleFor(p => p.PolicyholderBillingZipCode, f => f.Address.ZipCode())
            .RuleFor(p => p.PolicyholderBillingPostalCode, (_, p) => p.PolicyholderBillingZipCode)
            .RuleFor(p => p.PolicyholderBillingCountry, "USA")
            .RuleFor(p => p.ConsentType, "As per signed Practice Policy");
    }
}