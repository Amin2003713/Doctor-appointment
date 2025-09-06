using App.Applications.DrugStore.Responses;

namespace App.Applications.Prescriptions.Requests;

public class CreatePrescriptionItemRequest
{
    // Inferred from API code for body.Items
    public Guid? DrugId { get; set; }
    public string? DrugName { get; set; }
    public string Dosage { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string? Instructions { get; set; }
    public bool IsPRN { get; set; }
    public int RefillCount { get; set; }

    public static CreatePrescriptionItemRequest ToPrescriptionItem(
        DrugResponse drug,
        string dosage,
        string frequency,
        string duration,
        string? instructions = null,
        bool isPrn = false,
        int refillCount = 0)
    {
        if (drug == null) throw new ArgumentNullException(nameof(drug));

        return new CreatePrescriptionItemRequest
        {
            DrugId       = drug.Id,
            DrugName     = BuildDrugDisplayName(drug), // safe human-readable label
            Dosage       = dosage,
            Frequency    = frequency,
            Duration     = duration,
            Instructions = instructions,
            IsPRN        = isPrn,
            RefillCount  = refillCount
        };
    }

    private static string BuildDrugDisplayName(DrugResponse drug)
    {
        var main = !string.IsNullOrWhiteSpace(drug.BrandName)
            ? drug.BrandName
            : !string.IsNullOrWhiteSpace(drug.GenericName)
                ? drug.GenericName
                : drug.Id.ToString();

        // combine strength + form nicely
        var strength = drug.StrengthValue.HasValue
            ? $"{drug.StrengthValue} {drug.StrengthUnit}".Trim()
            : drug.ConcentrationText;

        var form = drug.Form.ToString();

        var extras = new[]
        {
            strength,
            form
        };

        var extraText = string.Join(" ", extras.Where(s => !string.IsNullOrWhiteSpace(s)));

        return string.IsNullOrWhiteSpace(extraText) ? main : $"{main} — {extraText}";
    }
}