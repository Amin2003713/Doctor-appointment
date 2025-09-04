// Note: This implementation assumes a client-side .NET project using MediatR for CQRS patterns (where requests inherit from IRequest<TResponse>),
// FluentValidation for validation with Persian error messages, and Refit for API clients.
// All DTOs are replicated based on the provided API code.
// Enums are inferred and defined where necessary.
// Validators are created for request classes (which wrap the API bodies/params).
// Refit interfaces are defined per controller.
// Handlers are not implemented as they were not requested, but these can be used in MediatR handlers to call the Refit APIs.

// First, define enums used across DTOs (inferred from API code)




// DTOs for Drugs
namespace App.Applications.DrugStore.Requests;

public class UpsertDrugRequest
{
    public string BrandName { get; set; } = string.Empty;
    public string GenericName { get; set; } = string.Empty;
    public int Form { get; set; }
    public int Route { get; set; }
    public decimal? StrengthValue { get; set; }
    public string? StrengthUnit { get; set; }
    public string? ConcentrationText { get; set; }
    public int RxClass { get; set; }
    public string? Manufacturer { get; set; }
    public string? Country { get; set; }
    public string? Barcode { get; set; }
    public string? Tags { get; set; }
    public List<string>? Synonyms { get; set; }
    public bool IsActive { get; set; }
}