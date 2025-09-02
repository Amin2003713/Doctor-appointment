namespace Api.Endpoints.Models.Drugs;

public class DrugSynonym
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DrugId { get; set; }
    public string Text { get; set; } = default!;
}