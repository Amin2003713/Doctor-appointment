using Refit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Api.Client.Models;
using App.Applications.Appointments.Requests;
using FluentValidation;
using MediatR;

namespace Api.Client.Models
{

    public enum PrescriptionStatus
    {
        Issued,
        Dispensed,
        Cancelled
    }

    public enum IssueMethod
    {
        Paper,
        Electronic
    }

    public enum DrugForm
    {
        Tablet,
        Capsule,
        Injection,
        Syrup,
        Cream,
        Ointment,
        Drops,
        Inhaler,
        Patch,
        Suppository
    }

    public enum DrugRoute
    {
        Oral,
        Intravenous,
        Intramuscular,
        Topical,
        Inhalation,
        Rectal,
        Ophthalmic,
        Otic,
        Nasal,
        Subcutaneous
    }

    public enum RxClass
    {
        Prescription,
        OTC,
        Controlled
    }

    // Drug Models
    public class DrugResponse
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; } = null!;
        public string GenericName { get; set; } = null!;
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
        public List<string> Synonyms { get; set; } = new();
        public bool IsActive { get; set; }
    }

    public class UpsertDrugRequest
    {
        public string BrandName { get; set; } = null!;
        public string GenericName { get; set; } = null!;
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

    public class DrugAutocompleteItem
    {
        public Guid Id { get; set; }
        public string GenericName { get; set; } = null!;
        public int Form { get; set; }
        public int Route { get; set; }
        public string Label { get; set; } = null!;
    }

    public class DrugSearchResult
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<DrugResponse> Items { get; set; } = new();
    }

    // Prescription Models
    public class PrescriptionItemDto
    {
        public Guid Id { get; set; }
        public Guid? DrugId { get; set; }
        public string DrugName { get; set; } = null!;
        public string? GenericName { get; set; }
        public string Dosage { get; set; } = null!;
        public string Frequency { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string? Instructions { get; set; }
        public bool IsPRN { get; set; }
        public int RefillCount { get; set; }
    }

    public class PrescriptionResponse
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public long PatientUserId { get; set; }
        public long PrescribedByUserId { get; set; }
        public DateTime IssuedAtUtc { get; set; }
        public int Status { get; set; }
        public int IssueMethod { get; set; }
        public string? ErxCode { get; set; }
        public string? Notes { get; set; }
        public List<PrescriptionItemDto> Items { get; set; } = new();
    }

    public class CreatePrescriptionRequest
    {
        public Guid AppointmentId { get; set; }
        public long PatientUserId { get; set; }
        public string? Notes { get; set; }
        public int IssueMethod { get; set; }
        public List<PrescriptionItemDto> Items { get; set; } = new();
    }

    // Medical Record Models
    public class AppointmentSummary
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Start { get; set; } = null!;
        public string End { get; set; } = null!;
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
    }

    public class MedicalRecordResponse
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public string Notes { get; set; } = null!;
        public string? Diagnosis { get; set; }
        public string? PrescriptionText { get; set; }
        public string? AttachmentsUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpsertMedicalRecordRequest
    {
        public Guid AppointmentId { get; set; }
        public string Notes { get; set; } = null!;
        public string? Diagnosis { get; set; }
        public string? PrescriptionText { get; set; }
        public string? AttachmentsUrl { get; set; }
    }

    public class PatientEhrResponse
    {
        public long PatientUserId { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; }
        public List<AppointmentSummary> Appointments { get; set; } = new();
        public List<MedicalRecordResponse> MedicalRecords { get; set; } = new();
        public List<PrescriptionResponse> Prescriptions { get; set; } = new();
    }
}

namespace Api.Client
{
    [Headers("Authorization: Bearer")]
    public interface IApiClient
    {
        // DrugsController
        [Post("/api/drugs")]
        Task<DrugResponse> CreateDrugAsync([Body] UpsertDrugRequest request, CancellationToken ct = default);

        [Put("/api/drugs/{id}")]
        Task<DrugResponse> UpdateDrugAsync(Guid id, [Body] UpsertDrugRequest request, CancellationToken ct = default);

        [Get("/api/drugs/{id}")]
        Task<DrugResponse> GetDrugByIdAsync(Guid id, CancellationToken ct = default);

        [Get("/api/drugs/search")]
        Task<DrugSearchResult> SearchDrugsAsync(
            [Query] string? q,
            [Query] int? form,
            [Query] int? route,
            [Query] int? rxClass,
            [Query] bool? activeOnly,
            [Query] int page = 1,
            [Query] int pageSize = 20,
            CancellationToken ct = default);

        [Get("/api/drugs/autocomplete")]
        Task<List<DrugAutocompleteItem>> AutocompleteDrugsAsync(
            [Query] string q,
            [Query] int limit = 10,
            CancellationToken ct = default);

        [Get("/api/drugs/most-used")]
        Task<List<DrugAutocompleteItem>> GetMostUsedDrugsAsync(
            [Query] int days = 90,
            [Query] int limit = 15,
            CancellationToken ct = default);

        // PrescriptionsController
        [Post("/api/prescriptions")]
        Task<PrescriptionResponse> CreatePrescriptionAsync([Body] CreatePrescriptionRequest request, CancellationToken ct = default);

        [Get("/api/prescriptions/{id}")]
        Task<PrescriptionResponse> GetPrescriptionByIdAsync(Guid id, CancellationToken ct = default);

        [Get("/api/prescriptions/by-patient/{patientUserId}")]
        Task<List<PrescriptionResponse>> ListPrescriptionsByPatientAsync(long patientUserId, CancellationToken ct = default);

        [Put("/api/prescriptions/{id}/cancel")]
        Task CancelPrescriptionAsync(Guid id, CancellationToken ct = default);

        [Put("/api/prescriptions/{id}/dispense")]
        Task MarkPrescriptionDispensedAsync(Guid id, CancellationToken ct = default);

        // MedicalRecordsController
        [Get("/api/medical-records/{patientUserId}/ehr")]
        Task<PatientEhrResponse> GetEhrAsync(long patientUserId, CancellationToken ct = default);

        [Post("/api/medical-records")]
        Task<MedicalRecordResponse> UpsertMedicalRecordAsync([Body] UpsertMedicalRecordRequest request, CancellationToken ct = default);

        [Get("/api/medical-records/by-appointment/{appointmentId}")]
        Task<List<MedicalRecordResponse>> GetMedicalRecordsByAppointmentAsync(Guid appointmentId, CancellationToken ct = default);
    }
}


public record CreateDrugCommand(
    UpsertDrugRequest Request,
    Guid? UserId,
    string Role
) : IRequest<DrugResponse>;

public record UpdateDrugCommand(
    Guid Id,
    UpsertDrugRequest Request,
    Guid? UserId,
    string Role
) : IRequest<DrugResponse>;

public record GetDrugByIdQuery(
    Guid Id,
    Guid? UserId,
    string Role
) : IRequest<DrugResponse>;

public record SearchDrugsQuery(
    string? Query,
    int? Form,
    int? Route,
    int? RxClass,
    bool? ActiveOnly,
    int Page,
    int PageSize,
    Guid? UserId,
    string Role
) : IRequest<DrugSearchResult>;

public record AutocompleteDrugsQuery(
    string Query,
    int Limit,
    Guid? UserId,
    string Role
) : IRequest<List<DrugAutocompleteItem>>;

public record GetMostUsedDrugsQuery(
    int Days,
    int Limit,
    Guid? UserId,
    string Role
) : IRequest<List<DrugAutocompleteItem>>;

public record CreatePrescriptionCommand(
    CreatePrescriptionRequest Request,
    long? UserId
) : IRequest<PrescriptionResponse>;

public record GetPrescriptionByIdQuery(
    Guid Id,
    long? UserId,
    string Role
) : IRequest<PrescriptionResponse>;

public record ListPrescriptionsByPatientQuery(
    long PatientUserId,
    long? UserId,
    string Role
) : IRequest<List<PrescriptionResponse>>;

public record CancelPrescriptionCommand(
    Guid Id,
    long? UserId
) : IRequest;

public record MarkPrescriptionDispensedCommand(
    Guid Id
) : IRequest;

public record GetEhrQuery(
    long PatientUserId,
    long? UserId,
    string Role
) : IRequest<PatientEhrResponse>;

public record UpsertMedicalRecordCommand(
    UpsertMedicalRecordRequest Request,
    long? UserId,
    string Role
) : IRequest<MedicalRecordResponse>;

public record GetMedicalRecordsByAppointmentQuery(
    Guid AppointmentId,
    long? UserId,
    string Role
) : IRequest<List<MedicalRecordResponse>>;

 public class CreateDrugCommandValidator : AbstractValidator<CreateDrugCommand>
    {
        public CreateDrugCommandValidator()
        {
            RuleFor(x => x.Request.BrandName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Request.GenericName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Request.Form).Must(f => Enum.IsDefined(typeof(DrugForm), f)).WithMessage("Invalid Form.");
            RuleFor(x => x.Request.Route).Must(r => Enum.IsDefined(typeof(DrugRoute), r)).WithMessage("Invalid Route.");
            RuleFor(x => x.Request.RxClass).Must(r => Enum.IsDefined(typeof(RxClass), r)).WithMessage("Invalid RxClass.");
            RuleFor(x => x.Request.StrengthValue).GreaterThanOrEqualTo(0).When(x => x.Request.StrengthValue.HasValue);
            RuleFor(x => x.Request.StrengthUnit).MaximumLength(50).When(x => x.Request.StrengthUnit != null);
            RuleFor(x => x.Request.ConcentrationText).MaximumLength(100).When(x => x.Request.ConcentrationText != null);
            RuleFor(x => x.Request.Manufacturer).MaximumLength(100).When(x => x.Request.Manufacturer != null);
            RuleFor(x => x.Request.Country).MaximumLength(100).When(x => x.Request.Country != null);
            RuleFor(x => x.Request.Barcode).MaximumLength(50).When(x => x.Request.Barcode != null);
            RuleFor(x => x.Request.Tags).MaximumLength(500).When(x => x.Request.Tags != null);
            RuleForEach(x => x.Request.Synonyms).MaximumLength(100).When(x => x.Request.Synonyms != null);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary" }.Contains(r)).WithMessage("Role must be Doctor or Secretary.");
            RuleFor(x => x.UserId).NotNull().WithMessage("User ID required for Doctor/Secretary role.");
        }
    }

    public class UpdateDrugCommandValidator : AbstractValidator<UpdateDrugCommand>
    {
        public UpdateDrugCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
            RuleFor(x => x.Request.BrandName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Request.GenericName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Request.Form).Must(f => Enum.IsDefined(typeof(DrugForm), f)).WithMessage("Invalid Form.");
            RuleFor(x => x.Request.Route).Must(r => Enum.IsDefined(typeof(DrugRoute), r)).WithMessage("Invalid Route.");
            RuleFor(x => x.Request.RxClass).Must(r => Enum.IsDefined(typeof(RxClass), r)).WithMessage("Invalid RxClass.");
            RuleFor(x => x.Request.StrengthValue).GreaterThanOrEqualTo(0).When(x => x.Request.StrengthValue.HasValue);
            RuleFor(x => x.Request.StrengthUnit).MaximumLength(50).When(x => x.Request.StrengthUnit != null);
            RuleFor(x => x.Request.ConcentrationText).MaximumLength(100).When(x => x.Request.ConcentrationText != null);
            RuleFor(x => x.Request.Manufacturer).MaximumLength(100).When(x => x.Request.Manufacturer != null);
            RuleFor(x => x.Request.Country).MaximumLength(100).When(x => x.Request.Country != null);
            RuleFor(x => x.Request.Barcode).MaximumLength(50).When(x => x.Request.Barcode != null);
            RuleFor(x => x.Request.Tags).MaximumLength(500).When(x => x.Request.Tags != null);
            RuleForEach(x => x.Request.Synonyms).MaximumLength(100).When(x => x.Request.Synonyms != null);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary" }.Contains(r)).WithMessage("Role must be Doctor or Secretary.");
            RuleFor(x => x.UserId).NotNull().WithMessage("User ID required for Doctor/Secretary role.");
        }
    }

    public class GetDrugByIdQueryValidator : AbstractValidator<GetDrugByIdQuery>
    {
        public GetDrugByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary", "Patient" }.Contains(r))
                .WithMessage("Role must be Doctor, Secretary, or Patient.");
        }
    }

    public class SearchDrugsQueryValidator : AbstractValidator<SearchDrugsQuery>
    {
        public SearchDrugsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
            RuleFor(x => x.Query).MaximumLength(200).When(x => x.Query != null);
            RuleFor(x => x.Form).Must(f => Enum.IsDefined(typeof(DrugForm), f)).When(x => x.Form.HasValue);
            RuleFor(x => x.Route).Must(r => Enum.IsDefined(typeof(DrugRoute), r)).When(x => x.Route.HasValue);
            RuleFor(x => x.RxClass).Must(r => Enum.IsDefined(typeof(RxClass), r)).When(x => x.RxClass.HasValue);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary", "Patient" }.Contains(r))
                .WithMessage("Role must be Doctor, Secretary, or Patient.");
        }
    }

    public class AutocompleteDrugsQueryValidator : AbstractValidator<AutocompleteDrugsQuery>
    {
        public AutocompleteDrugsQueryValidator()
        {
            RuleFor(x => x.Query).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Limit).InclusiveBetween(1, 30);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary", "Patient" }.Contains(r))
                .WithMessage("Role must be Doctor, Secretary, or Patient.");
        }
    }

    public class GetMostUsedDrugsQueryValidator : AbstractValidator<GetMostUsedDrugsQuery>
    {
        public GetMostUsedDrugsQueryValidator()
        {
            RuleFor(x => x.Days).InclusiveBetween(1, 365);
            RuleFor(x => x.Limit).InclusiveBetween(1, 50);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary" }.Contains(r))
                .WithMessage("Role must be Doctor or Secretary.");
            RuleFor(x => x.UserId).NotNull().WithMessage("User ID required for Doctor/Secretary role.");
        }
    }

    public class CreatePrescriptionCommandValidator : AbstractValidator<CreatePrescriptionCommand>
    {
        public CreatePrescriptionCommandValidator()
        {
            RuleFor(x => x.Request.AppointmentId).NotEqual(Guid.Empty);
            RuleFor(x => x.Request.PatientUserId).GreaterThan(0);
            RuleFor(x => x.Request.Items).NotEmpty();
            RuleForEach(x => x.Request.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.DrugId).NotNull().When(i => string.IsNullOrWhiteSpace(i.DrugName))
                    .WithMessage("Either DrugId or DrugName must be provided.");
                item.RuleFor(i => i.DrugName).NotEmpty().When(i => !i.DrugId.HasValue)
                    .WithMessage("Either DrugId or DrugName must be provided.");
                item.RuleFor(i => i.Dosage).NotEmpty().MaximumLength(50);
                item.RuleFor(i => i.Frequency).NotEmpty().MaximumLength(50);
                item.RuleFor(i => i.Duration).NotEmpty().MaximumLength(50);
                item.RuleFor(i => i.Instructions).MaximumLength(500).When(i => i.Instructions != null);
                item.RuleFor(i => i.RefillCount).GreaterThanOrEqualTo(0);
            });
            RuleFor(x => x.Request.IssueMethod).Must(im => Enum.IsDefined(typeof(IssueMethod), im));
            RuleFor(x => x.Request.Notes).MaximumLength(2000).When(x => x.Request.Notes != null);
            RuleFor(x => x.UserId).NotNull().WithMessage("User ID required for Doctor role.");
        }
    }

    public class GetPrescriptionByIdQueryValidator : AbstractValidator<GetPrescriptionByIdQuery>
    {
        public GetPrescriptionByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary", "Patient" }.Contains(r))
                .WithMessage("Role must be Doctor, Secretary, or Patient.");
        }
    }

    public class ListPrescriptionsByPatientQueryValidator : AbstractValidator<ListPrescriptionsByPatientQuery>
    {
        public ListPrescriptionsByPatientQueryValidator()
        {
            RuleFor(x => x.PatientUserId).GreaterThan(0);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary", "Patient" }.Contains(r))
                .WithMessage("Role must be Doctor, Secretary, or Patient.");
            When(x => x.Role == "Patient", () =>
            {
                RuleFor(x => x.UserId).NotNull().Equal(x => x.PatientUserId)
                    .WithMessage("Patient can only access their own prescriptions.");
            });
        }
    }

    public class CancelPrescriptionCommandValidator : AbstractValidator<CancelPrescriptionCommand>
    {
        public CancelPrescriptionCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
            RuleFor(x => x.UserId).NotNull().WithMessage("User ID required for Doctor role.");
        }
    }

    public class MarkPrescriptionDispensedCommandValidator : AbstractValidator<MarkPrescriptionDispensedCommand>
    {
        public MarkPrescriptionDispensedCommandValidator()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
        }
    }

    public class GetEhrQueryValidator : AbstractValidator<GetEhrQuery>
    {
        public GetEhrQueryValidator()
        {
            RuleFor(x => x.PatientUserId).GreaterThan(0);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary", "Patient" }.Contains(r))
                .WithMessage("Role must be Doctor, Secretary, or Patient.");
            When(x => x.Role == "Patient", () =>
            {
                RuleFor(x => x.UserId).NotNull().Equal(x => x.PatientUserId)
                    .WithMessage("Patient can only access their own EHR.");
            });
        }
    }

    public class UpsertMedicalRecordCommandValidator : AbstractValidator<UpsertMedicalRecordCommand>
    {
        public UpsertMedicalRecordCommandValidator()
        {
            RuleFor(x => x.Request.AppointmentId).NotEqual(Guid.Empty);
            RuleFor(x => x.Request.Notes).NotEmpty().MaximumLength(5000);
            RuleFor(x => x.Request.Diagnosis).MaximumLength(1000).When(x => x.Request.Diagnosis != null);
            RuleFor(x => x.Request.PrescriptionText).MaximumLength(2000).When(x => x.Request.PrescriptionText != null);
            RuleFor(x => x.Request.AttachmentsUrl).MaximumLength(500).When(x => x.Request.AttachmentsUrl != null);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary" }.Contains(r))
                .WithMessage("Role must be Doctor or Secretary.");
            RuleFor(x => x.UserId).NotNull().WithMessage("User ID required for Doctor/Secretary role.");
        }
    }

    public class GetMedicalRecordsByAppointmentQueryValidator : AbstractValidator<GetMedicalRecordsByAppointmentQuery>
    {
        public GetMedicalRecordsByAppointmentQueryValidator()
        {
            RuleFor(x => x.AppointmentId).NotEqual(Guid.Empty);
            RuleFor(x => x.Role).Must(r => new[] { "Doctor", "Secretary", "Patient" }.Contains(r))
                .WithMessage("Role must be Doctor, Secretary, or Patient.");
        }
    }
