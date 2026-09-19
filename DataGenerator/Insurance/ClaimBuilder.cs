using System.Globalization;
using Bogus;
using DataGenerator.Insurance.Models;

namespace DataGenerator.Insurance;

internal class ClaimBuilder
{
    internal static readonly string[] Headers =
    [
        "Claim ID", "Client First name", "Client Last Name", "Claim Date",
        "Policy number", "Policyholder ID", "Pet ID", "Pet Name",
        "Insurance Brand", "Claim status", "Total Claimed Amount",
        "Total Allowed Amount", "Total Paid Amount"
    ];

    internal const string ClaimStatusApproved = "APPROVED";
    internal const string ClaimStatusInReview = "IN_REVIEW";
    internal const string ClaimStatusDenied = "DENIED";
    private static readonly  string[] ClaimStatuses = [ClaimStatusApproved, ClaimStatusInReview, ClaimStatusDenied];

    internal static List<Claim> GenerateData(
        List<Policy> policies,
        string dateFormat,
        int count)
    {
        var faker = new Faker();
        var claims = new List<Claim>();

        for (var i = 0; i < count; i++)
        {
            var claim = new Claim
            {
                ClaimId = $"cl{faker.Random.Int(1, 99999999):D8}"
            };

            var policy = FillDataFromRandomPolicy(policies, faker, claim);

            claim.ClaimStatus = faker.PickRandom(ClaimStatuses);
                
            // Date of claim in scope of policy dates
            var claimDate = DefineClaimDate(dateFormat, policy, faker);
            claim.ClaimDate = claimDate.ToString(dateFormat, CultureInfo.InvariantCulture);

            SetClaimAmounts(faker, claim);

            claims.Add(claim);
        }

        return claims;
    }

    private static void SetClaimAmounts(Faker faker, Claim claim)
    {
        var totalClaimAmount = Math.Round(faker.Random.Decimal(800, 1600), 2);
        var totalAllowedAmount = Math.Round(totalClaimAmount * faker.Random.Decimal(min: 0.7m, max: 1.0m), 2);
        var totalPaidAmount = Math.Round(totalAllowedAmount * faker.Random.Decimal(min: 0.5m, max: 1.0m), 2);

        claim.TotalClaimedAmount = totalClaimAmount.ToString(CultureInfo.InvariantCulture);
        claim.TotalAllowedAmount = totalAllowedAmount.ToString(CultureInfo.InvariantCulture);
        claim.TotalPaidAmount = totalPaidAmount.ToString(CultureInfo.InvariantCulture);
    }

    private static DateTime DefineClaimDate(string dateFormat, Policy policy, Faker faker)
    {
        var startDate = DateTime.ParseExact(policy.PolicyStartDate, dateFormat, CultureInfo.InvariantCulture);
        var endDate = DateTime.ParseExact(policy.PolicyEndDate, dateFormat, CultureInfo.InvariantCulture);
        var claimDate = faker.Date.Between(startDate, DateTime.Now < endDate ? DateTime.Now : endDate);
        return claimDate;
    }

    private static Policy FillDataFromRandomPolicy(List<Policy> policies, Faker faker, Claim claim)
    {
        var policy = faker.PickRandom(policies);
        claim.PolicyNumber = policy.PolicyNumber;
                
        claim.PolicyholderId = policy.PolicyholderId;
        claim.ClientFirstName = policy.PolicyholderFirstName;
        claim.ClientLastName = policy.PolicyholderLastName;

        claim.PetId = policy.PetId;
        claim.PetName = policy.PetName;

        claim.InsuranceBrand = policy.InsuranceBrand;
        return policy;
    }
}