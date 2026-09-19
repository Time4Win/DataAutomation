using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace DataGenerator.Insurance;

public class DataDirector
{
    private const string CustomDelimiter = "|";
    public void GenerateAllData(int totalRows, string dateFormat, string pathToFolder)
    {
        var timer = new Stopwatch();
        timer.Start();
        // Generates file name string in MMDDYYYY format
        var timestamp = DateTime.Now.ToString("MMddyyyy");

        // Generate initial data
        var policyholders = PolicyholderBuilder.GenerateData(totalRows);
        var pets = PetBuilder.GenerateData(totalRows, dateFormat);
        var providers = ProviderBuilder.GenerateData(totalRows);

        // Generate dependent data and pass basic lists
        var policies = PolicyBuilder.GenerateData(policyholders, pets, dateFormat, totalRows);
        var coverages = CoverageBuilder.GenerateData(policies, dateFormat, totalRows);
        var claims = ClaimBuilder.GenerateData(policies, dateFormat, totalRows);
        var claimDetails = ClaimDetailBuilder.GenerateData(claims, providers, totalRows);

        // Export data directly using headers mapped to matching component types
        SaveToCsv(policyholders, $"{pathToFolder}policyholder_{timestamp}.csv", PolicyholderBuilder.Headers);
        SaveToCsv(pets, $"{pathToFolder}pet_{timestamp}.csv", PetBuilder.Headers);
        SaveToCsv(providers, $"{pathToFolder}provider_{timestamp}.csv", ProviderBuilder.Headers);
        SaveToCsv(policies, $"{pathToFolder}policy_{timestamp}.csv", PolicyBuilder.Headers);
        SaveToCsv(coverages, $"{pathToFolder}coverage_{timestamp}.csv", CoverageBuilder.Headers);
        SaveToCsv(claims, $"{pathToFolder}claims_{timestamp}.csv", ClaimBuilder.Headers);
        SaveToCsv(claimDetails, $"{pathToFolder}claim_detail_{timestamp}.csv", ClaimDetailBuilder.Headers);

        timer.Stop();
        Console.WriteLine($"Data Generated for {totalRows} rows with {timer.Elapsed.TotalSeconds} seconds");
    }

    private static void SaveToCsv<T>(IEnumerable<T> data, string fileName, string[] headers)
    {
        using var writer = new StreamWriter(fileName);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = CustomDelimiter,
            HasHeaderRecord = false  // Disable automatic headers
        });

        // Write headers
        foreach (var header in headers)
        {
            csv.WriteField(header);
        }
        csv.NextRecord();

        // CsvHelper handles serialization
        csv.WriteRecords(data);
    }
}