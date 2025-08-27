using App.Common.Utilities.LifeTime;

namespace App.Common.General.States;

public class ApplicationDomain : BaseState ,
    ISingletonDependency
{
    private string? _domain;

    
    private bool _isDialogShowing;

    
    public Func<Task>? InvokeSelectCompanyDialog;

    public string? Domain
    {
        get
        {
            if (_domain != null)
                return _domain;

            
            if (!_isDialogShowing && InvokeSelectCompanyDialog != null)
            {
                _isDialogShowing = true;

                
                _ = InvokeSelectCompanyDialog()
                    .ContinueWith(t =>
                    {
                        
                        _isDialogShowing = false;

                        
                        if (t.IsFaulted)
                        {
                            
                        }
                    });
            }

            return null;
        }
        set => SetProperty(ref _domain , value);
    }
}




//





//









//


//





//











//






