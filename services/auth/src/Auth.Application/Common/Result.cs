using Auth.Application.Common;

namespace Auth.Application.Common;

public abstract class Result<T>
{
    public bool IsSuccess { get; protected set; }
    public AuthErrorCode ErrorCode { get; protected set; }
    public SuccessCode SuccessCode { get; protected set; }
    public string? ErrorMessage { get; protected set; }
    public T? Data { get; protected set; }

    protected Result()
    {
        IsSuccess = true;
        ErrorCode = AuthErrorCode.None;
        SuccessCode = SuccessCode.None;
    }

    public static SuccessResult<T> Success(T data, SuccessCode successCode = SuccessCode.OperationCompleted)
    {
        return new SuccessResult<T>(data, successCode);
    }

    public static ErrorResult<T> Error(AuthErrorCode errorCode, string? errorMessage = null)
    {
        return new ErrorResult<T>(errorCode, errorMessage);
    }

    public static ErrorResult<T> Error(AuthErrorCode errorCode)
    {
        return new ErrorResult<T>(errorCode);
    }
}

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data, SuccessCode successCode)
    {
        Data = data;
        IsSuccess = true;
        SuccessCode = successCode;
        ErrorCode = AuthErrorCode.None;
    }
}

public class ErrorResult<T> : Result<T>
{
    public ErrorResult(AuthErrorCode errorCode, string? errorMessage = null)
    {
        IsSuccess = false;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        SuccessCode = SuccessCode.None;
        Data = default;
    }
}
