using System.Runtime.ExceptionServices;
using App.Common.Exceptions;
using App.Common.General.ApiResult;
using App.Common.Utilities.LifeTime;
using App.Common.Utilities.Snackbar;
using App.Persistence.Services.AuthState;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace App.Persistence.Services.Exceptions
{
    public class ExceptionHandlerService : IExceptionNotifier
    {
        private readonly IDictionary<Type , Action<Exception>> _exceptionHandlers;
        private readonly ILogger<ExceptionHandlerService>      _logger;
        private readonly ClientStateProvider                   _stateProvider;
        private readonly ISnackbarService                      _snackbarService;

        private readonly IMemoryCache _memoryCache;

        // How many milliseconds to “cool down” repeated exceptions
        private const int ExceptionCooldownMilliseconds = 500;

        public ExceptionHandlerService(
            ILogger<ExceptionHandlerService> logger ,
            ClientStateProvider              stateProvider ,
            IMemoryCache                     memoryCache ,
            ISnackbarService                 snackbarService)
        {
            _logger          = logger;
            _stateProvider   = stateProvider;
            _memoryCache     = memoryCache;
            _snackbarService = snackbarService;

            _exceptionHandlers = new Dictionary<Type , Action<Exception>>
            {
                {
                    typeof(ShiftyException) , HandleShiftyException
                } ,
                {
                    typeof(ApiProblemDetails) , HandleApiProblemDetailsException
                } ,
                {
                    typeof(ArgumentNullException) , HandleArgumentNullException
                } ,
                {
                    typeof(ValidationException) , HandleValidationException
                } ,
                {
                    typeof(FluentValidation.ValidationException) , HandleFluentValidationException
                } ,
                {
                    typeof(NotFoundException) , HandleNotFoundException
                } ,
                {
                    typeof(ConflictException) , HandleConflictException
                } ,
                {
                    typeof(UnauthorizedAccessException) , HandleUnauthorizedException
                } ,
                {
                    typeof(HandledExceptions) , HandleHandledException
                } ,
                {
                    typeof(ForbiddenException) , HandleForbiddenException
                }
            };
        }

        private void HandleHandledException(Exception exception)
        {
            _logger.LogWarning("________________Handled Exception: {Message}___________________________" , exception.Message);
        }

        private void HandleApiProblemDetailsException(Exception context)
        {
            if (context is not ApiProblemDetails ex)
                return;

            _logger.LogWarning("________________Handled Exception: {Message}___________________________" , context.Message);

            switch (ex.Status)
            {
                case 401:
                    _snackbarService.ShowError(ex.Detail?.ToString() ?? "Unauthorized");
                    _ = _stateProvider.Logout();
                    break;
                case 403:
                    _snackbarService.ShowError(ex.Title?.ToString() ?? "Forbidden" + ex.Detail?.ToString() ?? "Access Denied");
                    break;
                case 404:
                    _snackbarService.ShowError(ex.Title);
                    break;
                case 400:
                    _snackbarService.ShowError(ex.Errors.FirstOrDefault().Value.FirstOrDefault() ?? "Bad Request");
                    break;
                default:
                    _snackbarService.ShowError(ex.Title);
                    break;
            }

            _logger.LogError(ex.Status , ex.Title);
        }

        private void HandleShiftyException(Exception context)
        {
            if (context is not ShiftyException exception)
                return;

            _logger.LogError(exception.Message);
        }

        private void HandleArgumentNullException(Exception exception)
        {
            _logger.LogWarning("ArgumentNullException: {Message}" , exception.Message);
        }

        private void HandleValidationException(Exception exception)
        {
            _snackbarService.ShowError("Validation Error: " + exception.Message);
            _logger.LogWarning("ValidationException: {Message}" , exception.Message);
        }

        private void HandleFluentValidationException(Exception exception)
        {
            _snackbarService.ShowError("Validation Error: " + exception.Message);
            _logger.LogWarning("FluentValidationException: {Message}" , exception.Message);
        }

        private void HandleNotFoundException(Exception exception)
        {
            _snackbarService.ShowError("Not Found: " + exception.Message);
            _logger.LogWarning("NotFoundException: {Message}" , exception.Message);
        }

        private void HandleConflictException(Exception exception)
        {
            _snackbarService.ShowError("Conflict" + exception.Message);
            _logger.LogWarning("ConflictException: {Message}" , exception.Message);
        }

        private void HandleUnauthorizedException(Exception exception)
        {
            _snackbarService.ShowError("Unauthorized" + "You are not authorized to perform this action.");
            _logger.LogWarning("UnauthorizedAccessException: {Message}" , exception.Message);
        }

        private void HandleForbiddenException(Exception exception)
        {
            _snackbarService.ShowError("Forbidden" + "Access to this resource is forbidden.");
            _logger.LogWarning("ForbiddenException: {Message}" , exception.Message);
        }

        private void HandleUnknownException(Exception exception)
        {
            _logger.LogError(exception , "Unhandled exception: {Message}" , exception.Message);
        }

        private void HandleException(object sender , Exception exception)
        {
            // 1. Determine if we should skip because the same exception “key” was handled too recently
            if (ShouldSkipException(exception))
            {
                // Don’t do anything if it’s within the cooldown
                return;
            }

            // 2. Mark as handled (store in cache)
            MarkExceptionHandled(exception);

            // 3. Run the main handler logic
            var exceptionType = exception.GetType();

            if (_exceptionHandlers.TryGetValue(exceptionType , out var handler))
            {
                handler.Invoke(exception);
                _logger.LogWarning(exception , "Handled exception of type {ExceptionType}" , exceptionType.Name);
                return;
            }

            HandleUnknownException(exception);
        }

        /// <summary>
        /// Returns true if the same exception (by type + message) was recently handled 
        /// within the last ExceptionCooldownMilliseconds. Otherwise false.
        /// </summary>
        private bool ShouldSkipException(Exception ex)
        {
            var key = BuildExceptionKey(ex);

            // If the cache contains this key, skip it
            return _memoryCache.TryGetValue(key , out _);
        }

        /// <summary>
        /// Store the exception key with a short expiration (500 ms by default).
        /// </summary>
        private void MarkExceptionHandled(Exception ex)
        {
            var key = BuildExceptionKey(ex);

            // We only need the key itself; the stored value doesn’t matter, just the existence.
            // Set an absolute expiration to remove it after the cooldown window.
            _memoryCache.Set(
                key ,
                DateTime.UtcNow ,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(ExceptionCooldownMilliseconds)
                }
            );
        }

        /// <summary>
        /// Generates a consistent key for the exception. 
        /// In this example, we use {Type.FullName} + {Message}.
        /// You can add stack trace or other details if needed.
        /// </summary>
        private static string BuildExceptionKey(Exception ex)
        {
            return $"{ex.GetType().FullName}::{ex.Message}";
        }

        // Methods from IExceptionNotifier
        public void Notify(object sender , UnhandledExceptionEventArgs e)
        {
            HandleException(sender , (e.ExceptionObject as Exception)!);
        }

        public void Notify(object? sender , FirstChanceExceptionEventArgs e)
        {
            HandleException(sender! , e.Exception);
        }

        public void Notify(Exception e)
        {
            // If e.Source is null or empty, just pass in a fallback string
            var source = string.IsNullOrEmpty(e.Source) ? "Unknown" : e.Source;
            HandleException(source , e);
        }
    }
}