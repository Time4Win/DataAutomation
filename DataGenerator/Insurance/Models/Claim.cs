namespace DataGenerator.Insurance.Models
{
    public class Claim
    {
        public string ClaimId { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClaimDate { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyholderId { get; set; }
        public string PetId { get; set; }
        public string PetName { get; set; }
        public string InsuranceBrand { get; set; }
        public string ClaimStatus { get; set; }
        public string TotalClaimedAmount { get; set; }
        public string TotalAllowedAmount { get; set; }
        public string TotalPaidAmount { get; set; }
    }
}