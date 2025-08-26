using System.Text.Json;
using System.Text.Json.Serialization;
using App.Common.General.ApiResult;
using Refit;

namespace App.Common.Utilities.Converter;

public static class ConverterExtensions
{
    public static TOut Deserialize<TOut>(this string content)
    {
        return JsonSerializer.Deserialize<TOut>(content ,
                   new JsonSerializerOptions
                   {
                       PropertyNameCaseInsensitive = true ,
                       DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                   }) ??
               default!;
    }


    public static TOut DeserializeErrors<TOut , TIn>(this ApiResponse<TIn> content)
    {
        return content.IsSuccessStatusCode ? default! : content.Error!.Content!.Deserialize<TOut>();
    }

    public static TOut DeserializeOrThrow<TOut>(this object content)
    {
        if (content is not IApiResponse<TOut> api)
            return default!;

        if (api.IsSuccessStatusCode)
            return api.Content!;

        throw api.DeserializeErrors<ApiProblemDetails>();
    }

    public static void DeserializeAndThrow(this IApiResponse response)
    {
        if (response.IsSuccessStatusCode)
            return ;

        throw response.DeserializeErrors<ApiProblemDetails>();
    }

    public static TOut DeserializeErrors<TOut>(this IApiResponse content)
    {
        return content.IsSuccessStatusCode ? default! : content.Error!.Content!.Deserialize<TOut>();
    }

    public static TOut Deserialize<TOut>(this IApiResponse<TOut> content)
    {
        return content.IsSuccessStatusCode ? content.Content! : default!;
    }
}