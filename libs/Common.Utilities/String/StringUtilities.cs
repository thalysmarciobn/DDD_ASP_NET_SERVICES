using System.Text;
using System.Text.RegularExpressions;

namespace Common.Utilities.String;

public static class StringUtilities
{
    public static string GenerateSlug(string text, int maxLength = 50)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var normalizedString = text.ToLowerInvariant();
        
        normalizedString = Regex.Replace(normalizedString, @"[^a-z0-9\s-]", "");
        normalizedString = Regex.Replace(normalizedString, @"\s+", "-");
        normalizedString = Regex.Replace(normalizedString, @"-+", "-");
        normalizedString = normalizedString.Trim('-');

        if (normalizedString.Length > maxLength)
        {
            normalizedString = normalizedString.Substring(0, maxLength).TrimEnd('-');
        }

        return normalizedString;
    }

    public static string TruncateWithEllipsis(string text, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrWhiteSpace(text) || text.Length <= maxLength)
            return text;

        return text.Substring(0, maxLength - suffix.Length) + suffix;
    }

    public static string ToTitleCase(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var result = new StringBuilder();

        for (int i = 0; i < words.Length; i++)
        {
            if (i > 0) result.Append(' ');
            
            if (words[i].Length > 0)
            {
                result.Append(char.ToUpper(words[i][0]));
                if (words[i].Length > 1)
                    result.Append(words[i].Substring(1).ToLower());
            }
        }

        return result.ToString();
    }

    public static string RemoveSpecialCharacters(string text, bool keepSpaces = true)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var pattern = keepSpaces ? @"[^a-zA-Z0-9\s]" : @"[^a-zA-Z0-9]";
        return Regex.Replace(text, pattern, "");
    }

    public static string GenerateRandomString(int length, bool includeUppercase = true, bool includeLowercase = true, bool includeNumbers = true, bool includeSpecialChars = false)
    {
        if (length <= 0)
            throw new ArgumentException("Length must be greater than 0", nameof(length));

        var chars = new StringBuilder();
        
        if (includeUppercase) chars.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        if (includeLowercase) chars.Append("abcdefghijklmnopqrstuvwxyz");
        if (includeNumbers) chars.Append("0123456789");
        if (includeSpecialChars) chars.Append("!@#$%^&*()_+-=[]{}|;:,.<>?");

        if (chars.Length == 0)
            throw new ArgumentException("At least one character type must be selected");

        var random = new Random();
        var result = new char[length];
        
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[random.Next(chars.Length)];
        }

        return new string(result);
    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        var cleaned = Regex.Replace(phoneNumber, @"[\s\-\(\)\+]", "");
        return Regex.IsMatch(cleaned, @"^\d{10,15}$");
    }

    public static string MaskEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            return email;

        var parts = email.Split('@');
        var username = parts[0];
        var domain = parts[1];

        if (username.Length <= 2)
            return email;

        var maskedUsername = username[0] + new string('*', username.Length - 2) + username[^1];
        return $"{maskedUsername}@{domain}";
    }

    public static string MaskPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return phoneNumber;

        var cleaned = Regex.Replace(phoneNumber, @"[\s\-\(\)\+]", "");
        
        if (cleaned.Length < 4)
            return phoneNumber;

        var visibleDigits = Math.Min(4, cleaned.Length / 3);
        var masked = cleaned.Substring(0, visibleDigits) + new string('*', cleaned.Length - visibleDigits * 2) + cleaned.Substring(cleaned.Length - visibleDigits);
        
        return masked;
    }
}
