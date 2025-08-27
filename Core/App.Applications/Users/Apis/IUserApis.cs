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
    using App.Common.General;
    using App.Common.General.ApiResult;
    using Refit;

#endregion

    namespace App.Applications.Users.Apis;

    public interface IUserApis
    {
        

        [Post(ApiRoutes.User.Register)]
        Task<ApiResponse<object>> Register([Body] RegisterApiRequest body);

        [Post(ApiRoutes.User.RegisterPatient)]
        Task<ApiResponse<object>> RegisterPatient([Body] RegisterApiRequest body);

        [Post(ApiRoutes.User.RegisterSecretary)]
        Task<ApiResponse<object>> RegisterSecretary([Body] RegisterApiRequest body);

        

        [Post(ApiRoutes.User.ChangeRole)]
        Task<ApiResponse<object>> ChangeRole([Body] ChangeRoleRequest body);

        [Post(ApiRoutes.User.ToggleTemplate)]
        Task<ApiResponse<UserInfoResponse>> Toggle([Body] ToggleUserRequest body);

        

        [Post(ApiRoutes.User.Login)]
        Task<ApiResponse<LoginResponse>> Login([Body] LoginRequest body);

        [Post(ApiRoutes.User.ForgotPassword)]
        Task<ApiResponse<object>> ForgotPassword([Body] ResetPasswordRequest body);


        [Get(ApiRoutes.User.Me)]
        Task<ApiResponse<UserInfoResponse>> Me();

        
        [Get(ApiRoutes.User.UserByIdTemplate)]
        Task<ApiResponse<UserInfoResponse>> GetUser(long id);

        
        [Get(ApiRoutes.User.Users)]
        Task<ApiResponse<PagedResult<UserListItemResponse>>> GetUsers([Query] UsersQueryRequest queryRequest);

        [Get(ApiRoutes.User.UsersSecretaries)]
        Task<ApiResponse<PagedResult<UserListItemResponse>>> GetSecretaries([Query] UsersQueryRequest queryRequest);


        [Multipart]
        [Post("/api/user/profile/avatar")]
        Task<ApiResponse<string>> UploadAvatar([AliasAs("file")] StreamPart file);

        [Put("/api/user/profile")]
        Task<ApiResponse<object>> UpdateProfile(UpdateProfileRequest body);

        [Put("/api/user/profile/{id}")]
        Task<ApiResponse<object>> UpdateUser(UpdateUserRequest body , long id);
    }