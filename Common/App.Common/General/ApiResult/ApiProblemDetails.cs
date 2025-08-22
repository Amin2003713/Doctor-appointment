using MediatR;

namespace App.Common.General.ApiResult;

public class ApiProblemDetails : Exception 
{
    public string Title { get; set; }
    public int Status { get; set; }
    public object Detail { get; set; }
    public Dictionary<string , List<string>> Errors { get; set; } = null!;
}