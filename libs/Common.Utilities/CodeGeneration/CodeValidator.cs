using System.Text.RegularExpressions;

namespace Common.Utilities.CodeGeneration;

public static class CodeValidator
{
    public static bool IsValidNumericCode(string code, int expectedLength = 6)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        if (code.Length != expectedLength)
            return false;

        return code.All(char.IsDigit);
    }

    public static bool IsValidAlphanumericCode(string code, int expectedLength = 8)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        if (code.Length != expectedLength)
            return false;

        return code.All(c => char.IsLetterOrDigit(c) && char.IsUpper(c));
    }

    public static bool IsValidMixedCode(string code, int expectedLength = 8)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        if (code.Length != expectedLength)
            return false;

        var hasLetter = false;
        var hasNumber = false;

        foreach (var c in code)
        {
            if (char.IsLetter(c))
                hasLetter = true;
            else if (char.IsDigit(c))
                hasNumber = true;
            else
                return false;
        }

        return hasLetter && hasNumber;
    }

    public static bool IsValidCustomCode(string code, string allowedChars, int expectedLength)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(allowedChars))
            return false;

        if (code.Length != expectedLength)
            return false;

        return code.All(c => allowedChars.Contains(c));
    }

    public static bool IsValidEmailVerificationCode(string code)
    {
        return IsValidNumericCode(code, 6);
    }

    public static bool IsValidPasswordResetCode(string code)
    {
        return IsValidAlphanumericCode(code, 8);
    }

    public static bool IsValidTwoFactorCode(string code)
    {
        return IsValidNumericCode(code, 6);
    }
}
