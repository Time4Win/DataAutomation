namespace DataGenerator.Insurance.Models
{
    public class ClaimDetail
    {
        public string ClaimDetailId { get; set; }
        public string ClaimId { get; set; }
        public string ClaimedAmount { get; set; }
        public string AllowedAmount { get; set; }
        public string PaidAmount { get; set; }
        public string CopayAmount { get; set; }
        public string DeniedAmount { get; set; }
        public string DeniedReason { get; set; }
        public string DeductibleAmount { get; set; }
        public string ChargedAmount { get; set; }
        public string Condition { get; set; }
        public string ServiceProviderId { get; set; }
    }
}