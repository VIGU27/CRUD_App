namespace CRUD_App.ResponseModels;

/// <summary>Standard response envelope returned by every endpoint.</summary>
public class ApiResponse<T>
{
    public bool IsError { get; set; }
    public int Code { get; set; }
    public string? Message { get; set; }
    public T? Result { get; set; }

    public static ApiResponse<T> Success(T result, int code, string message) => new()
    {
        IsError = false,
        Code = code,
        Message = message,
        Result = result
    };

    public static ApiResponse<T> Failure(int code, string message, T? result = default) => new()
    {
        IsError = true,
        Code = code,
        Message = message,
        Result = result
    };
}
