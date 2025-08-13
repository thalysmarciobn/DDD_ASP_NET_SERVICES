namespace Common.CQRS;

public abstract class Result<T>
{
    public bool IsSuccess { get; protected set; }
    public T? Data { get; protected set; }
    public int? ErrorCode { get; protected set; }
    public int? SuccessCode { get; protected set; }
    public string? ErrorMessage { get; protected set; }

    public static SuccessResult<T> Success(T data, int successCode)
    {
        return new SuccessResult<T>(data, successCode);
    }

    public static ErrorResult<T> Error(int errorCode, string? errorMessage = null)
    {
        return new ErrorResult<T>(errorCode, errorMessage);
    }
}

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data, int successCode)
    {
        IsSuccess = true;
        Data = data;
        SuccessCode = successCode;
    }
}

public class ErrorResult<T> : Result<T>
{
    public ErrorResult(int errorCode, string? errorMessage)
    {
        IsSuccess = false;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}
