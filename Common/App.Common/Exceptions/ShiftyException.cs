using System.Net;

namespace App.Common.Exceptions;

public class ShiftyException : Exception
{
    public ShiftyException(
        string message = null,
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError,
        Exception innerException = null,
        object additionalData = null)
        : base(message, innerException)
    {
        HttpStatusCode = httpStatusCode;
        AdditionalData = additionalData;
    }

    public HttpStatusCode HttpStatusCode { get; }
    public object AdditionalData { get; }


    public static ShiftyException Create(string message)
    {
        return new ShiftyException(message: message);
    }


    public static ShiftyException Create(string message, object additionalData)
    {
        return new ShiftyException(message, additionalData: additionalData);
    }


    public static ShiftyException Create(HttpStatusCode httpStatusCode, string message)
    {
        return new ShiftyException(httpStatusCode: httpStatusCode, message: message);
    }


    public static ShiftyException Create(
        string message,
        HttpStatusCode httpStatusCode,
        Exception innerException,
        object additionalData)
    {
        return new ShiftyException(message, httpStatusCode, innerException, additionalData);
    }

    public static HandledExceptions HandelExceptions()
    {
        return new HandledExceptions();
    }

#region Predefined API Exceptions

    public static NotFoundException NotFound(string message = "Resource not found.", object additionalData = null)
    {
        return new NotFoundException(message, additionalData);
    }


    public static UnauthorizedAccessException Unauthorized(string message = "Unauthorized access.", object additionalData = null)
    {
        return new UnauthorizedAccessException(message  + "\n" + additionalData );
    }


    public static ShiftyException BadRequest(string message = "Bad request.", object additionalData = null)
    {
        return new ShiftyException(message, HttpStatusCode.BadRequest, null, additionalData);
    }


    public static ShiftyException Validation(string message = "Validation failed.", object additionalData = null)
    {
        return new ShiftyException(message, HttpStatusCode.BadRequest, null, additionalData);
    }


    public static ConflictException Conflict(string message = "Conflict occurred.", object additionalData = null)
    {
        return new ConflictException(message, additionalData);
    }


    public static ShiftyException InternalServerError(string message = "An unexpected error occurred.", object additionalData = null)
    {
        return new ShiftyException(message, HttpStatusCode.InternalServerError, null, additionalData);
    }

    public static ForbiddenException Forbidden(string message , object additionalData = null)
    {
        return new ForbiddenException(message, null, additionalData);
    }


    public static NetworkException NetworkException()
    {
        return new NetworkException();
    }

#endregion
}