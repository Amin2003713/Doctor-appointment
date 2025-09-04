using App.Applications.DrugStore.Requests;
using App.Applications.DrugStore.Responses;
using MediatR;

namespace App.Applications.DrugStore.Commands;

public class CreateDrugCommand : IRequest<DrugResponse>
{
    public UpsertDrugRequest Body { get; set; } = new();
}