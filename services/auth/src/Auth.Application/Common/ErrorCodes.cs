namespace Auth.Application.Common;

public enum AuthErrorCode
{
    None = 0,
    UserNotFound = 1001,
    UserAlreadyExists = 1002,
    InvalidCredentials = 1003,
    UserInactive = 1004,
    InvalidInput = 1005,
    DatabaseError = 1006,
    ValidationError = 1007
}

public enum SuccessCode
{
    None = 0,
    UserRegistered = 2001,
    UserLoggedIn = 2002,
    UserRetrieved = 2003,
    OperationCompleted = 2004
}
