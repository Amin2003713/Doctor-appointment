using System.Net;
using System.Net.NetworkInformation;

namespace App.Common.Exceptions
{
    public class ShiftyException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }
        public object AdditionalData { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShiftyException"/> class with specified parameters.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="additionalData">Any additional data related to the exception.</param>
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

        // Static factory methods for common scenarios

        /// <summary>
        /// Creates a ShiftyException with a message.
        /// </summary>
        public static ShiftyException Create(string message) =>
            new ShiftyException(message: message);

        /// <summary>
        /// Creates a ShiftyException with a message and additional data.
        /// </summary>
        public static ShiftyException Create(string message, object additionalData) =>
            new ShiftyException(message: message, additionalData: additionalData);

        /// <summary>
        /// Creates a ShiftyException with a specific HTTP status code and message.
        /// </summary>
        public static ShiftyException Create(HttpStatusCode httpStatusCode, string message) =>
            new ShiftyException(httpStatusCode: httpStatusCode, message: message);

        /// <summary>
        /// Creates a ShiftyException with all parameters.
        /// </summary>
        public static ShiftyException Create(
            string message,
            HttpStatusCode httpStatusCode,
            Exception innerException,
            object additionalData) =>
            new ShiftyException(message, httpStatusCode, innerException, additionalData);

        #region Predefined API Exceptions

        /// <summary>
        /// Creates a NotFound ShiftyException (HTTP 404).
        /// </summary>
        public static NotFoundException NotFound(string message = "Resource not found.", object additionalData = null) =>
            new NotFoundException(message, additionalData);

        /// <summary>
        /// Creates an Unauthorized ShiftyException (HTTP 401).
        /// </summary>
        public static UnauthorizedAccessException Unauthorized(string message = "Unauthorized access.", object additionalData = null) =>
            new UnauthorizedAccessException(message  + "\n"+ additionalData );

        /// <summary>
        /// Creates a BadRequest ShiftyException (HTTP 400).
        /// </summary>
        public static ShiftyException BadRequest(string message = "Bad request.", object additionalData = null) =>
            new ShiftyException(message, HttpStatusCode.BadRequest, null, additionalData);

        /// <summary>
        /// Creates a Validation ShiftyException (HTTP 400) with validation errors.
        /// </summary>
        public static ShiftyException Validation(string message = "Validation failed.", object additionalData = null) =>
            new ShiftyException(message, HttpStatusCode.BadRequest, null, additionalData);

        /// <summary>
        /// Creates a Conflict ShiftyException (HTTP 409).
        /// </summary>
        public static ConflictException Conflict(string message = "Conflict occurred.", object additionalData = null) =>
            new ConflictException(message, additionalData);

        /// <summary>
        /// Creates an InternalServerError ShiftyException (HTTP 500).
        /// </summary>
        public static ShiftyException InternalServerError(string message = "An unexpected error occurred.", object additionalData = null) =>
            new ShiftyException(message, HttpStatusCode.InternalServerError, null, additionalData);
        
        public static ForbiddenException Forbidden(string message , object additionalData = null) =>
            new ForbiddenException(message, null, additionalData);


        public static NetworkException NetworkException() =>
            new NetworkException();
        #endregion

        public static HandledExceptions HandelExceptions()
            => new HandledExceptions();
    }

    
}