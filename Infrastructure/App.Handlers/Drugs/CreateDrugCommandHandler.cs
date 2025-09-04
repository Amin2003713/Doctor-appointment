using App.Applications.DrugStore.Apis;
using App.Applications.DrugStore.Commands;
using App.Applications.DrugStore.Responses;

public class CreateDrugCommandHandler (
    ApiFactory apiFactory
) : IRequestHandler<CreateDrugCommand, DrugResponse>
{
    public IDrugsApi Apis { get; set; } = apiFactory.CreateApi<IDrugsApi>();

    public async Task<DrugResponse> Handle(CreateDrugCommand request, CancellationToken cancellationToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            ArgumentNullException.ThrowIfNull(Apis, nameof(Apis));

            var result = await Apis.Create(request.Body);

            if (!result.IsSuccessStatusCode)
            {
                result.DeserializeAndThrow();
                return null!;
            }

            return result?.Content!;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}


// MedicalRecords Handlers


// Prescriptions Handlers