using System.Globalization;
using Bogus;
using DataGenerator.Insurance.Models;

namespace DataGenerator.Insurance;

internal class PolicyBuilder
{
    // Static header definition for CSV file export mapping
    internal static readonly string[] Headers =
    [
        "Policy Number", "Policyholder ID", "Policyholder First Name", "Policyholder Last Name",
        "Policy type", "Policy status", "Policy start date", "Policy end date",
        "Pet ID", "Pet Name", "Insurance Brand", "MGA assistance phone number",
        "MGA email for assistance", "Date of MGA file generation"
    ];

    private const string StatusActive = "ACTIVE";
    private const string StatusCancelled = "CANCELLED";
    private const string StatusLapsed = "LAPSED";
    private const string StatusExpired = "EXPIRED";

    private static readonly string[] PolicyStatuses =
        [StatusActive, StatusCancelled, StatusLapsed, StatusExpired];

    private static readonly string[] PolicyTypes = ["elite.cat", "V2_dog", "V3.3_cat", "premium.dog"];

    private const string MgaGenDateFormat = "MM/dd/yyyy HH:mm:ss";

    internal static List<Policy> GenerateData(List<Policyholder> policyholders, List<Pet> pets, string dateFormat, int count)
    {
        return GetGenerator(policyholders, pets, dateFormat)
            .Generate(count);
    }

    private static Faker<Policy> GetGenerator(List<Policyholder> policyholders, List<Pet> pets, string dateFormat)
    {
        var today = DateTime.Now;

        return new Faker<Policy>("en")
            // 1. CustomInstantiator handles interdependent fields (status, dates, and calculated generation timestamp)
            .CustomInstantiator(f =>
            {
                var policyholder = f.PickRandom(policyholders);
                var pet = f.PickRandom(pets);

                // Distribute statuses evenly (approx. 25% each)
                var status = f.PickRandom(PolicyStatuses);

                var startDate = GetStartDate(status, f, today);
                var endDate = startDate.AddYears(1);
                var mgaGenDate = startDate.AddDays(-f.Random.Number(1, 15));

                return new Policy
                {
                    PolicyholderId = policyholder.PolicyholderId,
                    PolicyholderFirstName = policyholder.PolicyholderFirstName,
                    PolicyholderLastName = policyholder.PolicyholderLastName,

                    PetId = pet.PetId,
                    PetName = pet.PetName,

                    PolicyStatus = status,

                    PolicyStartDate = startDate.ToString(dateFormat, CultureInfo.InvariantCulture),
                    PolicyEndDate = endDate.ToString(dateFormat, CultureInfo.InvariantCulture),
                    DateOfMgaFileGeneration = mgaGenDate.ToString(MgaGenDateFormat, CultureInfo.InvariantCulture)
                };
            })
            // 2. RuleFor chain handles independent fields
            .RuleFor(p => p.PolicyNumber, f => $"pl{f.IndexFaker + 1:D8}")
            .RuleFor(p => p.PolicyType, f => f.PickRandom(PolicyTypes))
            .RuleFor(p => p.InsuranceBrand, "MegaCompany")
            .RuleFor(p => p.MgaAssistancePhoneNumber, f => f.Phone.PhoneNumber("###-###-####"))
            .RuleFor(p => p.MgaEmailForAssistance, f => f.Internet.Email());
    }

    private static DateTime GetStartDate(string status, Faker f, DateTime today)
    {
        var startDate = status switch
        {
            StatusActive => f.Date.Between(today.AddYears(-1), today),
            StatusExpired => f.Date.Between(today.AddYears(-2), today.AddDays(-1)),
            _ => f.Date.Past(2)
        };

        return startDate;
    }
}