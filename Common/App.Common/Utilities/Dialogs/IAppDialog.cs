using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace App.Common.Utilities.Dialogs;

public interface IAppDialog
{
    Task<TResult?> ShowDialogAsync<TDialog , TResult>(
        string title ,
        DialogParameters? parameters = null ,
        DialogOptions? options = null)
        where TDialog : IComponent;

    Task<string> ShowDomainDialogAsync(
        string title ,
        DialogParameters? parameters = null ,
        DialogOptions? options = null);
}