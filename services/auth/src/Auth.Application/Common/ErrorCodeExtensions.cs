namespace Auth.Application.Common;

public static class ErrorCodeExtensions
{
    public static string GetMessage(this AuthErrorCode errorCode)
    {
        return errorCode switch
        {
            AuthErrorCode.None => "No error",
            AuthErrorCode.UserNotFound => "User not found",
            AuthErrorCode.UserAlreadyExists => "User already exists",
            AuthErrorCode.InvalidCredentials => "Invalid credentials",
            AuthErrorCode.UserInactive => "User is inactive",
            AuthErrorCode.InvalidInput => "Invalid input data",
            AuthErrorCode.DatabaseError => "Database error occurred",
            AuthErrorCode.ValidationError => "Validation error",
            _ => "Unknown error"
        };
    }

    public static string GetMessage(this SuccessCode successCode)
    {
        return successCode switch
        {
            SuccessCode.None => "No success",
            SuccessCode.UserRegistered => "User registered successfully",
            SuccessCode.UserLoggedIn => "User logged in successfully",
            SuccessCode.UserRetrieved => "User retrieved successfully",
            SuccessCode.OperationCompleted => "Operation completed successfully",
            _ => "Unknown success"
        };
    }
}
