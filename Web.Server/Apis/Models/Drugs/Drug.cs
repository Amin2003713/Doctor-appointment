namespace Api.Endpoints.Models.Drugs;

// OTC=بدون نسخه، Rx=نسخه‌ای، کنترل‌شده

/// <summary>
/// One row per concrete presentation (brand OR generic) + form + strength.
/// </summary>
public class Drug
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Naming
    public string BrandName { get; set; } = default!;   // eg. "Amoxil"
    public string GenericName { get; set; } = default!; // eg. "Amoxicillin"

    // Presentation
    public DrugForm Form { get; set; } = DrugForm.Tablet;
    public DrugRoute Route { get; set; } = DrugRoute.Oral;

    // Strength / Concentration
    public decimal? StrengthValue { get; set; }    // 500
    public string? StrengthUnit { get; set; }      // "mg"
    public string? ConcentrationText { get; set; } // "125 mg/5 mL" for liquids

    // Classification
    public RxClass RxClass { get; set; } = RxClass.Rx;

    // Manufacturer/Meta (optional)
    public string? Manufacturer { get; set; }
    public string? Country { get; set; }
    public string? Barcode { get; set; }

    // Search helpers
    public string? Tags { get; set; }                      // CSV keywords
    public bool IsActive { get; set; } = true;

    // Audit
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public ICollection<DrugSynonym> Synonyms { get; set; } = new List<DrugSynonym>();
}