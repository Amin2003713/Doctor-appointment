using Api.Endpoints.Models.ValueObjects;

namespace Api.Endpoints.Models.MediaclService;

public class MedicalService
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = default!;     
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public Money Price { get; set; } = new(0);
    public int VisitMinutes { get; set; }            
    public bool IsActive { get; set; } = true;
}