using System.Runtime.ExceptionServices;
using App.Common.Utilities.LifeTime;

public interface IExceptionNotifier : ITransientDependency
{
    void Notify(Exception exception);
    void Notify(object sender , UnhandledExceptionEventArgs e);
    void Notify(object sender , FirstChanceExceptionEventArgs e);
}