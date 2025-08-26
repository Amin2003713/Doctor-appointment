#region

    using App.Applications.Users.Requests.ChangeRoles;
    using App.Applications.Users.Requests.ForgotPassword;
    using App.Applications.Users.Requests.Login;
    using App.Applications.Users.Requests.Registers;
    using App.Applications.Users.Requests.ToggleUsers;
    using App.Applications.Users.Requests.UpdateUser;
    using App.Applications.Users.Requests.UserInfos;
    using App.Applications.Users.Requests.UserQueries;
    using App.Applications.Users.Response.Login;
    using App.Common.General.ApiResult;
    using Refit;

#endregion

    namespace App.Applications.Users.Apis;

    public interface IUserApis
    {
        // ----------- Auth / Register -----------

        [Post(ApiRoutes.Register)]
        Task<ApiResponse<object>> Register([Body] RegisterApiRequest body);

        [Post(ApiRoutes.RegisterPatient)]
        Task<ApiResponse<object>> RegisterPatient([Body] RegisterApiRequest body);

        [Post(ApiRoutes.RegisterSecretary)]
        Task<ApiResponse<object>> RegisterSecretary([Body] RegisterApiRequest body);

        // ----------- Role mgmt / Toggle -----------

        [Post(ApiRoutes.ChangeRole)]
        Task<ApiResponse<object>> ChangeRole([Body] ChangeRoleRequest body);

        [Post(ApiRoutes.ToggleTemplate)]
        Task<ApiResponse<UserInfoResponse>> Toggle([Body] ToggleUserRequest body);

        // ----------- Login / Forgot -----------

        [Post(ApiRoutes.Login)]
        Task<ApiResponse<LoginResponse>> Login([Body] LoginRequest body);

        [Post(ApiRoutes.ForgotPassword)]
        Task<ApiResponse<object>> ForgotPassword([Body] ResetPasswordRequest body);


        [Get(ApiRoutes.Me)]
        Task<ApiResponse<UserInfoResponse>> Me();

        // /api/auth/user/{id:long}
        [Get(ApiRoutes.UserByIdTemplate)]
        Task<ApiResponse<UserInfoResponse>> GetUser(long id);

        // /api/auth/users?page=1&pageSize=20&search=...
        [Get(ApiRoutes.Users)]
        Task<ApiResponse<PagedResult<UserListItemResponse>>> GetUsers([Query] UsersQueryRequest queryRequest);

        [Get(ApiRoutes.UsersSecretaries)]
        Task<ApiResponse<PagedResult<UserListItemResponse>>> GetSecretaries([Query] UsersQueryRequest queryRequest);


        [Multipart]
        [Post("/api/user/profile/avatar")]
        Task<ApiResponse<string>> UploadAvatar([AliasAs("file")] StreamPart file);

        [Put("/api/user/profile")]
        Task<ApiResponse<object>> UpdateProfile(UpdateProfileRequest body);

        [Put("/api/user/profile/{id}")]
        Task<ApiResponse<object>> UpdateUser(UpdateUserRequest body , long id);
    }