using System.Globalization;
using Bogus;
using DataGenerator.Insurance.Models;

namespace DataGenerator.Insurance;

public class ClaimDetailBuilder
{
    // Static header definition for CSV file export mapping
    internal static readonly string[] Headers =
    [
        "Claim Detail ID", "Claim ID", "Claimed amount", "Allowed amount",
        "Paid amount", "Copay amount", "Denied amount", "Denied reason",
        "Deductible amount", "Charged amount", "Condition", "Service Provider ID"
    ];

    private static readonly string[] DeniedReasons =
    [
        "", "Duplicate claim", "Missing documentation", "Not covered", "Out of network", "Policy lapsed",
    ];

    private static readonly string[] Conditions =
    [
        "Illness", "Injury", "Routine Care", "Surgery", "Infection"
    ];

    internal static List<ClaimDetail> GenerateData(
        List<Claim> claims,
        List<Provider> providers,
        int count)
    {
        var providerIds = providers.Select(p => p.ServiceProviderId).ToList();
        var faker = new Faker();
        var claimDetails = new List<ClaimDetail>();

        // Track processed Claim IDs to ensure that at least the first detail for an APPROVED claim has an empty DeniedReason
        var processedClaimIds = new HashSet<string>();

        for (var i = 0; i < count; i++)
        {
            // Randomly pick a claim to achieve a natural distribution of details per claim
            var claim = faker.PickRandom(claims);
                
            var claimedAmount = Math.Round(faker.Random.Decimal(240, 460), 2);
            var allowedAmount = Math.Round(claimedAmount * faker.Random.Decimal(0.7m, 1.0m), 2);
            var paidAmount = Math.Round(allowedAmount * faker.Random.Decimal(0.5m, 1.0m), 2);
                
            var claimDetail = new ClaimDetail
            {
                ClaimDetailId = $"cd{faker.Random.Int(1, 99999999):D8}",
                ClaimId = claim.ClaimId,
                ClaimedAmount = claimedAmount.ToString(CultureInfo.InvariantCulture),
                AllowedAmount = allowedAmount.ToString(CultureInfo.InvariantCulture),
                PaidAmount = paidAmount.ToString(CultureInfo.InvariantCulture),
                CopayAmount = Math.Round(faker.Random.Decimal(14, 26), 2).ToString(CultureInfo.InvariantCulture),
                DeniedAmount = Math.Round(faker.Random.Decimal(0.0m, 0.5m), 2).ToString(CultureInfo.InvariantCulture),
                DeniedReason = GetRelevantDeniedReason(claim, processedClaimIds, faker),
                DeductibleAmount = Math.Round(faker.Random.Decimal(35, 65), 2).ToString(CultureInfo.InvariantCulture),
                ChargedAmount = claimedAmount.ToString(CultureInfo.InvariantCulture),
                Condition = faker.PickRandom(Conditions),
                ServiceProviderId = faker.PickRandom(providerIds)
            };

            claimDetails.Add(claimDetail);
            processedClaimIds.Add(claim.ClaimId);
        }

        return claimDetails;
    }

    private static string GetRelevantDeniedReason(Claim claim, HashSet<string> processedClaimIds, Faker faker)
    {
        string deniedReason;
        // Business logic: if claim is APPROVED, ensure the first detail has no denial reason
        if (claim.ClaimStatus == ClaimBuilder.ClaimStatusApproved && !processedClaimIds.Contains(claim.ClaimId))
        {
            deniedReason = "";
        }
        else
        {
            deniedReason = faker.PickRandom(DeniedReasons);
        }

        return deniedReason;
    }
}