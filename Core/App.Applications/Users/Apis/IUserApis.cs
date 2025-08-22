#region

using App.Applications.Users.Requests.ForgotPassword;
using App.Applications.Users.Requests.Login;
using App.Applications.Users.Requests.SendActivationCode;
using App.Applications.Users.Requests.Verify;
using App.Applications.Users.Response.Login;
using App.Applications.Users.Response.SendActivationCode;
using App.Applications.Users.Response.Verify;
using App.Common.General;
using Refit;

#endregion

namespace App.Applications.Users.Apis;

public interface IUserApis
{
    /// <summary>login by username and password</summary>
    /// <returns>A <see cref="Task" /> that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown when the request returns a non-success status code.</exception>
    [Post("/api/user/login")]
    Task<ApiResponse<LoginResponse>> Login([Body] LoginRequest body , [Header(ApplicationConstants.Headers.UserName)] string userName);


    /// <summary>get new refresh and access token</summary>
    /// <returns>A <see cref="Task" /> that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown when the request returns a non-success status code.</exception>
    [Post("/api/user/refresh-token")]
    Task<ApiResponse<LoginResponse>> RefreshToken([Body] RefreshTokenRequest body);

    /// <summary>Send an activation code to the specified user.</summary>
    /// <returns>OK</returns>
    /// <exception cref="ApiException">
    ///     Thrown when the request returns a non-success status code:
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Status</term>
    ///             <description>Description</description>
    ///         </listheader>
    ///         <item>
    ///             <term>400</term>
    ///             <description>Bad Request</description>
    ///         </item>
    ///         <item>
    ///             <term>404</term>
    ///             <description>Not Found</description>
    ///         </item>
    ///         <item>
    ///             <term>500</term>
    ///             <description>Internal Server Error</description>
    ///         </item>
    ///     </list>
    /// </exception>
    [Get("/api/user/send-activation-code")]
    Task<ApiResponse<SendActivationCodeResponse>> Code([Body] SendActivationCodeRequest body);

    /// <summary>Verify the code sent to the user's phone number.</summary>
    /// <returns>OK</returns>
    /// <exception cref="ApiException">
    ///     Thrown when the request returns a non-success status code:
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Status</term>
    ///             <description>Description</description>
    ///         </listheader>
    ///         <item>
    ///             <term>400</term>
    ///             <description>Bad Request</description>
    ///         </item>
    ///         <item>
    ///             <term>500</term>
    ///             <description>Internal Server Error</description>
    ///         </item>
    ///     </list>
    /// </exception>
    [Put("/api/user/verify")]
    Task<ApiResponse<VerifyPhoneNumberResponse>> Verify([Body] VerifyPhoneNumberRequest body);


    /// <summary>
    /// Initiates a "forgot password" process by sending a reset link/code to the user's email.
    /// </summary>
    /// <param name="request">An object containing the user's email.</param>
    /// <param name="userName">As the header to work</param>
    /// <returns>A response indicating whether the email was sent successfully.</returns>
    /// <response code="200">Returns a success message indicating an email was sent (or that the email does not exist).</response>
    /// <response code="400">If the request is invalid (e.g., missing or invalid email address).</response>
    /// <response code="403">If an error occurs with password.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [Put("/api/user/forgot-password")]
    Task<IApiResponse>  ForgotPassword([Body] ForgotPasswordRequest request , [Header(ApplicationConstants.Headers.UserName)] string userName);

}