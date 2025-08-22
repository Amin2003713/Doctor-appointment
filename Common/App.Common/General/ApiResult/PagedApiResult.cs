#region

using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

namespace App.Common.General.ApiResult;

public class PagedApiResult<T>(
    T data ,
    int? total = null ,
    HttpStatusCode statusCode = HttpStatusCode.OK , // Default to HTTP OK
    string[] messages = null! ,
    string[] errors = null!)
    : IDisposable
{
    public T Data { get; } = data;
    public int? Total { get; private set; } = total;
    public HttpStatusCode StatusCode { get; private set; } = statusCode;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[] Messages { get; private set; } = messages is { Length: > 0 } ? messages : null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[] Errors { get; private set; } = errors is { Length: > 0 } ? errors : null!;

    public void Dispose()
    {
        if (Data is IDisposable disposableData)
        {
            disposableData.Dispose();
        }
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this ,
            new JsonSerializerOptions
            {
                WriteIndented = true ,
            });
    }
}

public class PagedApiResult : PagedApiResult<object>
{
    public PagedApiResult(
        object data ,
        int? total = null ,
        HttpStatusCode statusCode = HttpStatusCode.OK ,
        string[] messages = null! ,
        string[] errors = null!
    ) : base(data , total , statusCode , messages , errors)
    {
    }
}