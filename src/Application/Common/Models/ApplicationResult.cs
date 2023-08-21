namespace Application.Common.Models;
public class ApplicationResult<T>
{
    internal ApplicationResult(bool succeeded, T? result, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Result = result;
    }

    public bool Succeeded { get; init; }
    
    public T? Result { get; init; }

    public string[] Errors { get; init; }

    public static Result Success<T>(T? result)
    {
        return new Result(true, result, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, null, errors);
    }
}