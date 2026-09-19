using System.Globalization;
using Bogus;
using DataGenerator.Insurance.Models;

namespace DataGenerator.Insurance;

internal class PetBuilder
{
    // Static header definition for CSV file export mapping
    internal static readonly string[] Headers =
    [
        "Pet ID", "Pet Name", "Pet Species Code", "Pet Species Name", "Pet Breed", "Pet Breed Group",
        "Pet Age", "Pet Date of Birth", "Pet Deceased Flag", "Pet Gender", "Pet Age at Enrollment",
        "Pet Microchipped", "Pet Microchip ID", "Pet Color", "Pet Weight", "Weight UoM"
    ];

    internal static readonly string[] SpeciesCodes = ["DOG", "CAT"];

    internal static List<Pet> GenerateData(int count, string dateFormat)
    {
        return GetGenerator(dateFormat)
            .Generate(count);
    }

    private static Faker<Pet> GetGenerator(string dateFormat)
    {
        return new Faker<Pet>("en")
            .RuleFor(p => p.PetId, f => $"pt{f.IndexFaker + 1:D8}")
            .RuleFor(p => p.PetName, f => f.Name.FirstName())
            .RuleFor(p => p.PetSpeciesCode, f => f.PickRandom(SpeciesCodes))
            .RuleFor(p => p.PetSpeciesName, (_, p) => p.PetSpeciesCode)
            .RuleFor(p => p.PetBreed, "Dog-PureB")
            .RuleFor(p => p.PetBreedGroup, "Sporting")
            .RuleFor(p => p.PetAge, f => f.Random.Number(1, 12))
            .RuleFor(p => p.PetDateOfBirth, (f, p) => f.Random.Bool(0.2f) ? "" : f.Date.Past(p.PetAge).ToString(dateFormat, CultureInfo.InvariantCulture))
            .RuleFor(p => p.PetDeceasedFlag, f => f.PickRandom("Y", "N"))
            .RuleFor(p => p.PetGender, f => f.PickRandom("M", "F"))
            .RuleFor(p => p.PetAgeAtEnrollment, "5 Months")
            .RuleFor(p => p.PetMicrochipped, f => f.PickRandom("Y", "N"))
            .RuleFor(p => p.PetMicrochipId, (f, p) => p.PetMicrochipped == "Y" ? f.Random.Replace("mc12345#####") : "")
            .RuleFor(p => p.PetColor, "Brown")
            .RuleFor(p => p.PetWeight, f => f.Random.Bool(0.1f) ? "" : Math.Round(f.Random.Double(4.0, 25.0), 2).ToString(CultureInfo.InvariantCulture))
            .RuleFor(p => p.WeightUoM, "kg");
    }
}