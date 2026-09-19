using System.Globalization;
using Bogus;
using DataGenerator.Insurance.Models;

namespace DataGenerator.Insurance;

internal class CoverageBuilder
{
    // List of headers matching the data columns for export or display
    public static readonly string[] Headers =
    [
        "Coverage ID", "Policy Number", "Pet ID", "Coverage Type",
        "Coverage Start Date", "Coverage Percentage", "Limit Type", "Limit Amount",
        "Used Limit", "Amount Left to meet deductible", "Add-on"
    ];

    private const string LimitTypeDollar = "Dollar Amount";
    private const string LimitTypeQuantity = "Quantity";
    private static readonly string[] CoverageTypes = 
    [
        "ProHeart 6", "Heartworm Test", "Other Fees", "Wellness Exam"
    ];

    private static readonly string[] AddOns = ["Wellness", "Standard", "None"];

    internal static List<Coverage> GenerateData(List<Policy> policies, string dateFormat, int count)
    {
        //Create a pool of unique combinations of PetID and CoverageType to ensure no duplicates for the same pet
        var availableCombinations = BuildAvailableCoverageCombinations(policies);

        // Create a dictionary for quick lookup of all policies belonging to a specific pet
        var petToPoliciesMap = policies.GroupBy(p => p.PetId).ToDictionary(g => g.Key, g => g.ToList());

        var faker = GetBaseDataFaker();
        var result = new List<CoverageDraft>();
        var targetCount = Math.Min(count, availableCombinations.Count);
        Random randomizer = new();
        
        //Generate coverages safely from the unique pool
        for (var i = 0; i < targetCount; i++)
        {
            var combination = availableCombinations[0];
            availableCombinations.RemoveAt(0);

            var petId = combination.PetId;
            var coverageType = combination.CoverageType;

            // Pick a random policy belonging to this specific pet
            var petPolicies = petToPoliciesMap[petId];
            var selectedPolicy = petPolicies[randomizer.Next(petPolicies.Count)];

            // Generate base fields via Faker
            var coverage = faker.Generate();

            // Assign proper relationships and type
            coverage.PetID = petId;
            coverage.PolicyNumber = selectedPolicy.PolicyNumber;
            coverage.CoverageType = coverageType;

            // Parse policy dates and generate coverage start date within that range (нарешті на місці!)
            var startDate = DateTime.ParseExact(selectedPolicy.PolicyStartDate, dateFormat, CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(selectedPolicy.PolicyEndDate, dateFormat, CultureInfo.InvariantCulture);

            var dateFaker = new Faker();
            coverage.CoverageStartDate = dateFaker.Date.Between(startDate, endDate).ToString(dateFormat);

            result.Add(coverage);
        }

        return ConvertToListCoverage(result);
    }

    private static Faker<CoverageDraft> GetBaseDataFaker()
    {
        return new Faker<CoverageDraft>("en")
            .RuleFor(c => c.CoverageID, f => $"cv{f.IndexFaker + 1:D8}")
            .RuleFor(c => c.CoveragePercentage, f => f.Random.Number(64, 97).ToString())
            .RuleFor(c => c.LimitType, f => f.PickRandom(LimitTypeDollar, LimitTypeQuantity))

            .RuleFor(c => c.LimitAmount, (f, c) =>
                c.LimitType == LimitTypeQuantity
                    ? f.Random.Number(1, 20)
                    : Math.Round(f.Random.Decimal(1000, 15000), 2))

            .RuleFor(c => c.UsedLimit, (f, c) =>
                c.LimitType == LimitTypeQuantity
                    ? f.Random.Number(0, (int)c.LimitAmount)
                    : Math.Round(f.Random.Decimal(100, c.LimitAmount * 0.8m), 2))

            .RuleFor(c => c.AmountLeftToMeetDeductible, (_, c) => c.LimitAmount - c.UsedLimit)
            .RuleFor(c => c.AddOn, f => f.PickRandom(AddOns));
    }

    private static List<(string PetId, string CoverageType)> BuildAvailableCoverageCombinations(List<Policy> policies)
    {
        // 1. Gather unique PetIDs from all policies
        var uniquePetIds = policies.Select(p => p.PetId).Distinct().ToList();

        // 2. Build a pool of all possible unique combinations (PetID -> CoverageType) to prevent duplicates
        var availableCombinations = new List<(string PetId, string CoverageType)>();
        foreach (var petId in uniquePetIds)
        {
            foreach (var coverageType in CoverageTypes)
            {
                availableCombinations.Add((petId, coverageType));
            }
        }

        // 3. Shuffle the pool for randomness
        Random random = new();
        availableCombinations = availableCombinations.OrderBy(_ => random.Next()).ToList();

        Console.WriteLine($"Total unique combinations pet ID => Coverage Type available: {availableCombinations.Count}");

        return availableCombinations;
    }

    private static List<Coverage> ConvertToListCoverage(List<CoverageDraft> drafts) 
    {
        var result = drafts.Select(d => new Coverage
        {
            CoverageID = d.CoverageID,
            PolicyNumber = d.PolicyNumber,
            PetID = d.PetID,
            CoverageType = d.CoverageType,
            CoverageStartDate = d.CoverageStartDate,
            CoveragePercentage = d.CoveragePercentage, 
            LimitType = d.LimitType,
            LimitAmount = d.LimitType == LimitTypeQuantity
                ? d.LimitAmount.ToString("0.0", CultureInfo.InvariantCulture)
                : d.LimitAmount.ToString("0.00", CultureInfo.InvariantCulture),
            UsedLimit = d.LimitType == LimitTypeQuantity
                ? d.UsedLimit.ToString("0.0", CultureInfo.InvariantCulture)
                : d.UsedLimit.ToString("0.00", CultureInfo.InvariantCulture),
            AmountLeftToMeetDeductible = d.LimitType == LimitTypeQuantity
                ? d.AmountLeftToMeetDeductible.ToString("0.0", CultureInfo.InvariantCulture)
                : d.AmountLeftToMeetDeductible.ToString("0.00", CultureInfo.InvariantCulture),
            AddOn = d.AddOn
        }).ToList();

        return result;
    }
}