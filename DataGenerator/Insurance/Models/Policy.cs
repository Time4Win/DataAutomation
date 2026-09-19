namespace DataGenerator.Insurance.Models
{
    public class Policy
    {
        public string PolicyNumber { get; set; }
        public string PolicyholderId { get; set; }
        public string PolicyholderFirstName { get; set; }
        public string PolicyholderLastName { get; set; }
        public string PolicyType { get; set; }
        public string PolicyStatus { get; set; }
        public string PolicyStartDate { get; set; }
        public string PolicyEndDate { get; set; }
        public string PetId { get; set; }
        public string PetName { get; set; }
        public string InsuranceBrand { get; set; }
        public string MgaAssistancePhoneNumber { get; set; }
        public string MgaEmailForAssistance { get; set; }
        public string DateOfMgaFileGeneration { get; set; }
    }
}