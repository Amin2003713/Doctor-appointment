namespace App.Common.Exceptions;

public class ConflictException : Exception
{
    public ConflictException() { }

    public ConflictException(string message)
        : base(message) { }

    public ConflictException(string message , Exception innerException)
        : base(message , innerException) { }

    public ConflictException(string name , object key)
        : base($"${name} {key}") { }
}