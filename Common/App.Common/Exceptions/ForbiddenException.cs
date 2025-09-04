using System.Net;

namespace App.Common.Exceptions;

public class ForbiddenException : ShiftyException
{
    public ForbiddenException(string message , Exception innerException , object additionalData = null)
        : base(message ,
            HttpStatusCode.Forbidden ,
            innerException ,
            additionalData) { }
}