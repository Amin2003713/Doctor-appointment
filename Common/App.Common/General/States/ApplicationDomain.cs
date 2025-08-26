using App.Common.Utilities.LifeTime;

namespace App.Common.General.States;

public class ApplicationDomain : BaseState ,
    ISingletonDependency
{
    private string? _domain;

    // Flag to block multiple invocations of the dialog
    private bool _isDialogShowing;

    // This delegate should be assigned to a function that shows your dialog.
    public Func<Task>? InvokeSelectCompanyDialog;

    public string? Domain
    {
        get
        {
            if (_domain != null)
                return _domain;

            // Only invoke the dialog if it is not already being shown.
            if (!_isDialogShowing && InvokeSelectCompanyDialog != null)
            {
                _isDialogShowing = true;

                // Fire-and-forget the dialog invocation.
                _ = InvokeSelectCompanyDialog()
                    .ContinueWith(t =>
                    {
                        // Reset the flag regardless of success or failure.
                        _isDialogShowing = false;

                        // Optionally handle exceptions here:
                        if (t.IsFaulted)
                        {
                            // Log the error or take additional actions as needed.
                        }
                    });
            }

            return null;
        }
        set => SetProperty(ref _domain , value);
    }
}

// public class AppStates : INotifyPropertyChanged , ISingletonDependency
// {
//     public event PropertyChangedEventHandler? PropertyChanged;
//
//     private void OnPropertyChanged(string propertyName)
//     {
//         PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propertyName));
//         _ = UpdateTenant();
//     }
//
//     private async Task UpdateTenant()
//     {
//         if (!string.IsNullOrEmpty(_domain))
//             await _mediator.Send(new UpdateTenantCommand()
//             {
//                 Domain        = _domain ,
//                 ActiveProfile = true ,
//             });
//     }
//
//     private          string?   _domain;
//     private readonly IMediator _mediator;
//
//     public AppStates(IMediator mediator)
//     {
//         _mediator = mediator;
//         _         = LoadTenantAsync();
//     }
//
//     public string? Domain
//     {
//         get => _domain;
//         set
//         {
//             if (_domain == value)
//                 return;
//             _domain = value!;
//             OnPropertyChanged(nameof(Domain));
//         }
//     }
//
//     // Fetch Tenant Data from Database
//     public async Task LoadTenantAsync()
//     {
//         var tenant = await _mediator.Send(new GetActiveDomainQuery());
//         Domain = tenant != null ? tenant.Domain : string.Empty;
//     }
// }