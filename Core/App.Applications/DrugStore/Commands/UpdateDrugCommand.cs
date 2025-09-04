using App.Applications.DrugStore.Requests;
using App.Applications.DrugStore.Responses;
using MediatR;

namespace App.Applications.DrugStore.Commands;

public class UpdateDrugCommand : IRequest<DrugResponse>
{
    public Guid Id { get; set; }
    public UpsertDrugRequest Body { get; set; } = new();
}