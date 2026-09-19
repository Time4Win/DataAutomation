namespace DataGenerator.Insurance.Models
{
    public class Pet
    {
        public string PetId { get; set; }
        public string PetName { get; set; }
        public string PetSpeciesCode { get; set; }
        public string PetSpeciesName { get; set; }
        public string PetBreed { get; set; }
        public string PetBreedGroup { get; set; }
        public int PetAge { get; set; }
        public string PetDateOfBirth { get; set; }
        public string PetDeceasedFlag { get; set; }
        public string PetGender { get; set; }
        public string PetAgeAtEnrollment { get; set; }
        public string PetMicrochipped { get; set; }
        public string PetMicrochipId { get; set; }
        public string PetColor { get; set; }
        public string PetWeight { get; set; } // string, easy to return empty value
        public string WeightUoM { get; set; }
    }
}
