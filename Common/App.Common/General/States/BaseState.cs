using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App.Common.General.States;

public abstract class BaseState : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;


    protected bool SetProperty<T>(ref T backingField , T value , [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(backingField , value))
            return false;

        backingField = value;
        OnPropertyChanged(propertyName);
        return true;
    }


    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propertyName));
    }
}