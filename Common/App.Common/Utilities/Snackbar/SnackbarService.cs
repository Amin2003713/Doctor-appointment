#region

using System.Net;
using App.Common.General;
using MudBlazor;

#endregion

namespace App.Common.Utilities.Snackbar;

public class SnackbarService(ISnackbar snackbar) : ISnackbarService
{
    public void ShowApiResult(HttpStatusCode code , Action<SnackbarOptions>? configure = null!)
    {
        snackbar.Add(GetDefaultMessage(code) , GetSeverity(code) , configure);
    }

    public void ShowSuccess(string message = null! , Action<SnackbarOptions>? configure = null!)
    {
        Show(Severity.Success , message);
    }

    public void ShowError(string message = null! , Action<SnackbarOptions>? configure = null!)
    {
        Show(Severity.Error , message);
    }

    public void ShowWarning(string message = null! , Action<SnackbarOptions>? configure = null!)
    {
        Show(Severity.Warning , message);
    }

    public void ShowInfo(string message = null! , Action<SnackbarOptions>? configure = null!)
    {
        Show(Severity.Info , message);
    }

    public void ShowNotFound(string message = null! , Action<SnackbarOptions>? configure = null!)
    {
        Show(HttpStatusCode.NotFound , message);
    }

    public void ShowUnauthorized(string message = null! , Action<SnackbarOptions>? configure = null!)
    {
        Show(HttpStatusCode.Unauthorized , message);
    }

    private void Show(HttpStatusCode code = default , string? message = null! , Action<SnackbarOptions>? configure = null!)
    {
        snackbar.Add(message ?? GetDefaultMessage(code) , GetSeverity(code) , configure);
    }

    private void Show(Severity severity = default , string message = null! , Action<SnackbarOptions>? configure = null!)
    {
        snackbar.Add(message ?? "" , severity , configure);
    }

    private Severity GetSeverity(HttpStatusCode type)
    {
        return type switch
        {
            HttpStatusCode.Accepted or HttpStatusCode.Created or HttpStatusCode.NoContent => Severity.Error ,
            HttpStatusCode.InternalServerError or HttpStatusCode.BadGateway => Severity.Error ,
            HttpStatusCode.BadRequest or HttpStatusCode.NotFound or HttpStatusCode.NotAcceptable => Severity.Warning ,
            HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => Severity.Error , _ => Severity.Normal ,
        };
    }

    private string GetDefaultMessage(HttpStatusCode type)
    {
        return type switch
        {
            HttpStatusCode.Accepted or HttpStatusCode.Created or HttpStatusCode.NoContent => ApplicationConstants.HttpStatusMessages.SuccessMessage ,
            HttpStatusCode.InternalServerError or HttpStatusCode.BadGateway => ApplicationConstants.HttpStatusMessages.ErrorMessage ,
            HttpStatusCode.BadRequest => ApplicationConstants.HttpStatusMessages.ErrorMessage ,
            HttpStatusCode.NotFound => ApplicationConstants.HttpStatusMessages.NotFoundMessage ,
            HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => ApplicationConstants.HttpStatusMessages.UnauthorizedMessage , _ => "" ,
        };
    }
}