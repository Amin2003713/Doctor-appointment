#region

using System.Net;
using App.Common.Utilities.LifeTime;
using MudBlazor;

#endregion

namespace App.Common.Utilities.Snackbar;

public interface ISnackbarService : IScopedDependency
{
    void ShowApiResult(HttpStatusCode code , Action<SnackbarOptions>? configure = null!);
    void ShowSuccess(string message = null! , Action<SnackbarOptions>? configure = null!);
    void ShowError(string message = null! , Action<SnackbarOptions>? configure = null!);
    void ShowWarning(string message = null! , Action<SnackbarOptions>? configure = null!);
    void ShowInfo(string message = null! , Action<SnackbarOptions>? configure = null!);
    void ShowNotFound(string message = null! , Action<SnackbarOptions>? configure = null!);
    void ShowUnauthorized(string message = null! , Action<SnackbarOptions>? configure = null!);
}