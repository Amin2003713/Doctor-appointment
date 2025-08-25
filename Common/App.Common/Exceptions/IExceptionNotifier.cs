using System.Runtime.ExceptionServices;
using App.Common.Utilities.LifeTime;

namespace App.Common.Exceptions;

public interface IExceptionNotifier : ITransientDependency
{
    void Notify(Exception exception);
    void Notify(object sender , UnhandledExceptionEventArgs e);
    void Notify(object sender , FirstChanceExceptionEventArgs e);
}