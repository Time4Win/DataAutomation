# Pet Insurance Test Data Generator

## Overview

The **Pet Insurance Test Data Generator** is a .NET 8 console application designed to generate realistic test data for pet insurance systems. It uses the **Bogus** library to create 1000-2000 synthetic records that maintain referential integrity and business logic constraints across multiple related entities.

---

## Features

✅ **Realistic Data Generation** - Uses Bogus library for authentic, contextual data  
✅ **Referential Integrity** - Proper relationships between policyholders, pets, policies, claims, and coverages  
✅ **Business Logic** - Enforces business rules (e.g., claim dates within policy dates)  
✅ **Quick Generation** - Generates 1000-2000 records in 1-4 seconds  
✅ **Configurable Output** - Customizable date formats and row counts  
✅ **Professional CSV Export** - Uses CsvHelper with pipe-delimited format (`|`)  
✅ **SOLID Principles** - Clean, maintainable architecture  

---

## Architecture

### **Core Components**

| Component | Purpose |
|-----------|---------|
| **PolicyholderBuilder** | Generates policyholders with contact & address info |
| **PetBuilder** | Generates pets with species, breed, age, health data |
| **ProviderBuilder** | Generates service providers (veterinary clinics) |
| **PolicyBuilder** | Generates insurance policies linked to policyholders & pets |
| **CoverageBuilder** | Generates coverage details with unique pet+type combinations |
| **ClaimBuilder** | Generates insurance claims with status & amounts |
| **ClaimDetailBuilder** | Generates claim line items linked to claims & providers |
| **DataDirector** | Orchestrates generation and CSV export |

### **Data Flow**

```
Independent Data:           Dependent Data:
  Policyholder    ┐           Policy
  Pet             ├──────→    Coverage
  Provider        ┘           Claim
							  ClaimDetail
```

---

## Technologies

- **.NET 8** - Target framework
- **Bogus 35.x** - Fake data generation
- **CsvHelper 33.x** - CSV file handling with custom delimiters
- **System.Globalization** - Locale-aware formatting

---

## Generated Data Structure

### **Output Files** (one per entity type)

When `totalRows = 1000`, each entity generates exactly 1000 records:

```
policyholder_MMddyyyy.csv      → 1,000 policyholder records (21 columns)
pet_MMddyyyy.csv               → 1,000 pet records (16 columns)
provider_MMddyyyy.csv          → 1,000 provider records (11 columns)
policy_MMddyyyy.csv            → 1,000 policy records (14 columns)
coverage_MMddyyyy.csv          → 1,000 coverage records (11 columns)
claims_MMddyyyy.csv            → 1,000 claim records (13 columns)
claim_detail_MMddyyyy.csv      → 1,000 claim detail records (12 columns)
```

### **Format**

- **Delimiter:** Pipe character (`|`)
- **Header:** Custom business-friendly column names
- **Encoding:** UTF-8
- **Date Format:** Configurable (default: `MM/dd/yyyy`)
- **Numbers:** Decimal format with proper precision

---

## Sample Data Generated

### **Policyholder**
```
Policyholder ID|Policyholder First Name|Policyholder Last Name|Policyholder Email
ph00000001|John|Smith|john.smith@example.com
ph00000002|Sarah|Johnson|sarah.johnson@example.com
```

### **Pet**
```
Pet ID|Pet Name|Pet Species Code|Pet Species Name|Pet Age|Pet Gender
pt00000001|Fluffy|DOG|Dog|3|M
pt00000002|Whiskers|CAT|Cat|5|F
```

### **Policy**
```
Policy Number|Policyholder ID|Pet ID|Policy Status|Policy Start Date|Policy End Date
pl00000001|ph00000001|pt00000001|ACTIVE|01/15/2024|01/15/2025
pl00000002|ph00000002|pt00000002|EXPIRED|06/20/2023|06/20/2024
```

### **Claim**
```
Claim ID|Policy Number|Pet ID|Claim Status|Total Claimed Amount|Total Paid Amount
cl00000001|pl00000001|pt00000001|APPROVED|1200.50|900.00
cl00000002|pl00000002|pt00000002|DENIED|450.25|0.00
```

---

## Usage

### **Basic Usage**

```csharp
var director = new DataDirector();
director.GenerateAllData(
	totalRows: 1500,             // Number of records per entity (1000-2000 recommended)
	dateFormat: "MM/dd/yyyy",    // Custom date format
	pathToFolder: "C:/output/"   // Output directory
);
```

### **Output Example**

```
Data Generated for 1500 rows in 2.78 seconds
```

Generated files:
- `policyholder_01152025.csv` (1,500 records)
- `pet_01152025.csv` (1,500 records)
- `provider_01152025.csv` (1,500 records)
- `policy_01152025.csv` (1,500 records)
- `coverage_01152025.csv` (1,500 records)
- `claims_01152025.csv` (1,500 records)
- `claim_detail_01152025.csv` (1,500 records)

---

## Data Relationships

### **Referential Integrity**

- **Policyholder ↔ Pet:** Many-to-many relationship
  - 1 policyholder can have **multiple pets** (multiple policies)
  - 1 pet can have **multiple policyholders** (multiple policies)
- **Policy → Policyholder:** Each policy belongs to exactly one policyholder
- **Policy → Pet:** Each policy covers exactly one pet
- **Coverage → Policy:** Each coverage type links to a policy (unique pet+type combo)
- **Policy → Claim:** **1 policy can have multiple claims**
- **Claim → ClaimDetail:** Multiple claim details per claim
- **ClaimDetail → Provider:** Each claim detail references a provider

### **Business Logic**

- **Claim dates** fall within policy start/end dates
- **Coverage dates** fall within policy start/end dates
- **No duplicate coverages** for the same pet (enforced via combination pool)
- **Claim amounts** follow logical progression:
  - Claimed Amount ≥ Allowed Amount ≥ Paid Amount
- **Claim statuses** distributed evenly (`APPROVED`, `IN_REVIEW`, `DENIED`)
- **Policy statuses** distributed evenly (`ACTIVE`, `CANCELLED`, `LAPSED`, `EXPIRED`)

---

## Performance

| Records | Time | Total Records Generated | Performance |
|---------|------|------------------------|-------------|
| 1,000 per entity | 1.86 seconds | 7,000 (7 files) | ~3,763 records/sec |
| 2,000 per entity | 3.7 seconds | 14,000 (7 files) | ~3,784 records/sec |
| CSV export (7 files) | Included in total time | No separate overhead | RFC 4180 compliant with `\|` delimiter |

**Tested on:** Visual Studio Community 2026 with .NET 8

---

## Code Quality

- ✅ **SOLID Principles** - Single Responsibility, Dependency Inversion
- ✅ **No Magic Strings** - Constants properly organized
- ✅ **Efficient Lookups** - O(1) performance, single object lookup per record
- ✅ **Consistent Style** - File-scoped namespaces, modern C# 11+ features
- ✅ **Clean Architecture** - Easy to understand and maintain
- ✅ **Extensible** - Easy to add new entities or modify logic

---

## Requirements

- **.NET 8 SDK** or later
- **Visual Studio 2026** (or compatible IDE)
- **NuGet packages:**
  - `Bogus` >= 35.0.0
  - `CsvHelper` >= 33.0.0

---

## Installation

1. Clone/download the solution
2. Open `DataAutomation.slnx` in Visual Studio
3. Build the solution
4. Run the console application

---

## Configuration

Modify `Program.cs` or `DataDirector.cs` to customize:

```csharp
// Change these parameters
var totalRows = 1500;               // Records per entity (1000-2000 recommended)
var dateFormat = "MM/dd/yyyy";      // Custom date format
var outputPath = "C:/data/output/"; // Output directory
```

---

## Example Output Structure

```
C:/data/output/
├── policyholder_01152025.csv
├── pet_01152025.csv
├── provider_01152025.csv
├── policy_01152025.csv
├── coverage_01152025.csv
├── claims_01152025.csv
└── claim_detail_01152025.csv
```

Each file is ready to:
- ✅ Import into test databases
- ✅ Load into data warehouse staging areas
- ✅ Test API endpoints with realistic data
- ✅ Validate business logic and calculations
- ✅ Manual testing of UI workflows
- ✅ Training and demo environments

---

## Common Use Cases

1. **Development Testing** - Populate dev databases with realistic pet insurance data
2. **API Testing** - Generate predictable test data for REST API validation
3. **Reporting Testing** - Verify business reports with synthetic data
4. **Data Pipeline Testing** - Test ETL transformations with complete datasets
5. **Manual QA** - Seed test environments for exploratory testing
6. **Training** - Create demo data for training environments
7. **Database Schema Validation** - Verify all constraints and relationships

---

## Data Simplifications & Assumptions

The generator creates realistic test data, but with some intentional simplifications per customer requirements:

### **Policy Status vs Claim Status Independence**
- **Real World:** Claims can only exist on ACTIVE policies
- **Test Data:** Claims can exist on ANY policy status (ACTIVE, EXPIRED, CANCELLED, LAPSED)
- **Reason:** Testing system behavior with edge cases and historical claims

### **Other Simplifications**
- **Pet Species:** Limited to DOG and CAT (not all possible species)
- **Breeds:** Simplified to single breed "Dog-PureB" for consistency
- **Coverage Types:** Limited to 4 types (ProHeart 6, Heartworm Test, Other Fees, Wellness Exam)
- **Denied Reasons:** Fixed set of common reasons
- **Conditions:** Limited to 5 common service conditions (Illness, Injury, Routine Care, Surgery, Infection)
- **Amounts:** Random ranges within expected bounds, not strictly validated against real rates

### **Why These Simplifications?**
✅ Makes test data predictable and reproducible  
✅ Focuses on core business logic validation  
✅ Avoids complexity of complete business rule enforcement  
✅ Allows testing of edge cases (e.g., claims on expired policies)  
✅ Keeps data generation fast (1-4 seconds for 1000-2000 records)  

### **Production Use Considerations**
⚠️ This test data should **NOT** be used in production  
⚠️ Real insurance data must enforce all business rules  
⚠️ Actual claim processing requires strict policy status validation  

---

## Troubleshooting

### Issue: "Coverage record count different from totalRows"
**Solution:** Coverage generates exactly `totalRows` records, but ensures no duplicate pet+coverage type combinations. The builder creates a pool of all possible unique combinations (up to `pets × 4 coverage types`), then takes the first `min(totalRows, availableCombinations)`. This prevents the same pet from having duplicate coverage types.

### Issue: Date format mismatch
**Solution:** Ensure `dateFormat` parameter matches your database/system requirements. Default is `MM/dd/yyyy`.

### Issue: CSV files not created
**Solution:** Verify the output path exists and you have write permissions.

### Issue: Generation takes longer than expected
**Solution:** Check system resources. Typical performance for comparison:
Generates 7 files by 1000 rows within 1.86 seconds 
(When RAM is 32 GB, processor - 13th Gen Intel(R) Core(TM) i7 )

---

## Architecture Notes

The generator follows enterprise design patterns:

### **Design Patterns**

- **Builder Pattern** - Each entity (PolicyholderBuilder, PetBuilder, etc.) encapsulates all generation logic
  - Easy to maintain and modify generation rules for a specific entity
  - New builders can be added without affecting existing ones

- **Director Pattern** - DataDirector orchestrates the overall generation workflow
  - Controls the sequence: Policyholders → Pets → Providers → Policies → Coverages → Claims → ClaimDetails
  - Manages CSV export for all entities
  - Single entry point for test data generation

### **Extensibility**

The architecture is designed for future enhancements:
- ✅ **Add new builder classes** for additional entities without modifying existing code
- ✅ **Extend existing builders** with additional rules (e.g., new claim statuses, coverage types)
- ✅ **Modify generation logic** in individual builders independently
- ✅ **Add new validation rules** by incorporating business logic into builders

### **Performance & Quality**

- **Single object lookup** - No O(n²) searches; each record picks dependencies once
- **Constants** - No magic strings; all values organized in arrays
- **YAGNI** - No over-engineering; keeps code pragmatic and simple
- **CSV Handling** - Uses CsvHelper for proper escaping and RFC 4180 compliance

---

## License

Internal Use - Pet Insurance Testing

---

## Author

Mariia Kyrnytska — Time4Win

---

**Last Updated:** September 2026  
**Version:** 1.0  
**Tested On:** 1,000-2,000 records per entity