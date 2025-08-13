namespace Email.Application.Common;

public enum EmailErrorCode : int
{
    None = 0,
    EmailNotFound = 2001,
    VerificationCodeExpired = 2002,
    VerificationCodeInvalid = 2003,
    EmailAlreadyVerified = 2004,
    MaxAttemptsExceeded = 2005,
    EmailSendFailed = 2006,
    InvalidInput = 2007,
    DatabaseError = 2008
}

public enum EmailSuccessCode : int
{
    None = 0,
    VerificationEmailSent = 3001,
    VerificationEmailResent = 3002,
    EmailVerified = 3003,
    OperationCompleted = 3004
}
