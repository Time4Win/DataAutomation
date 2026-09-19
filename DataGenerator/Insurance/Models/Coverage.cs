
public class Coverage
{
    public string CoverageID { get; set; }
    public string PolicyNumber { get; set; }
    public string PetID { get; set; }
    public string CoverageType { get; set; }
    public string CoverageStartDate { get; set; }
    public string CoveragePercentage { get; set; }
    public string LimitType { get; set; }  // "Dollar Amount" or "Quantity"
    public string LimitAmount { get; set; } // For Quantity format, ends with .0
    public string UsedLimit { get; set; }
    public string AmountLeftToMeetDeductible { get; set; } // Calculated difference: Limit - Used
    public string AddOn { get; set; }
}

public class CoverageDraft
{
    public string CoverageID { get; set; }
    public string PolicyNumber { get; set; }
    public string PetID { get; set; }
    public string CoverageType { get; set; }
    public string CoverageStartDate { get; set; }
    public string CoveragePercentage { get; set; }
    public string LimitType { get; set; } // "Dollar Amount" or "Quantity"
    public decimal LimitAmount { get; set; } // For Quantity format, ends with .0
    public decimal UsedLimit { get; set; }
    public decimal AmountLeftToMeetDeductible { get; set; } // Calculated difference: Limit - Used
    public string AddOn { get; set; }
}