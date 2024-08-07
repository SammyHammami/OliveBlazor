namespace OliveBlazor.Core.Application.Entities;

public class OperationResult<T>
{
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }


    public T Data { get; set; }
}