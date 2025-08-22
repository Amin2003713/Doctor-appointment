using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Refit;
using App.Applications.Users.Apis;
using App.Common.General;
using App.Common.Utilities.LifeTime;

namespace App.Persistence.Services.Refit;

public class ApiFactory : ISingletonDependency
{
    private HttpClient HttpClient { get; }

    public ApiFactory(RefitDelegatingHandler refitHttpExceptionHandler)
    {
        ArgumentNullException.ThrowIfNull(refitHttpExceptionHandler);

        HttpClient = new HttpClient(refitHttpExceptionHandler)
        {
            BaseAddress = new Uri(ApplicationConstants.Server.BaseUrl) ,
            Timeout     = TimeSpan.FromSeconds(60) ,
        };
    }

    public T CreateApi<T>() where T : class
    {
        return RestService.For<T>(HttpClient , ApplicationConstants.Server.RefitSettings);
    }

    public object CreateApiClient(string interfaceName)
    {
        if (string.IsNullOrWhiteSpace(interfaceName))
            throw new ArgumentException("Interface name cannot be null or empty." , nameof(interfaceName));

        var interfaceType = typeof(IUserApis).Assembly.GetTypes().FirstOrDefault(t => t.IsInterface && t.Name == interfaceName);

        if (interfaceType == null)
            throw new InvalidOperationException($"Interface '{interfaceName}' not found in the assembly.");


        var createApiMethod = typeof(ApiFactory).GetMethod(nameof(CreateApi) , BindingFlags.Public | BindingFlags.Instance);
        if (createApiMethod == null)
            throw new InvalidOperationException("CreateApi method not found.");

        var genericMethod = createApiMethod.MakeGenericMethod(interfaceType);

        return genericMethod.Invoke(this , null)!;
    }
}