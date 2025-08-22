using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace App.Common.General.States;

public abstract class BaseState : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///     Sets the property and raises the PropertyChanged event if the value has changed.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="backingField">Reference to the backing field.</param>
    /// <param name="value">The new value.</param>
    /// <param name="propertyName">Name of the property (automatically provided by CallerMemberName).</param>
    /// <returns>True if the value was changed, false otherwise.</returns>
    protected bool SetProperty<T>(ref T backingField , T value , [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(backingField , value))
            return false;

        backingField = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    ///     Raises the PropertyChanged event for the specified property.
    /// </summary>
    /// <param name="propertyName">Name of the property (automatically provided by CallerMemberName).</param>
    private void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propertyName));
    }
}