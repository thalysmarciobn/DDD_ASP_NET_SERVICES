using System.Security.Cryptography;

namespace Common.Utilities.CodeGeneration;

public static class VerificationCodeGenerator
{
    private static readonly Random _random = new Random();
    private static readonly object _lockObject = new object();

    public static string GenerateNumericCode(int length = 6)
    {
        if (length <= 0 || length > 10)
            throw new ArgumentException("Length must be between 1 and 10", nameof(length));

        lock (_lockObject)
        {
            var minValue = (int)Math.Pow(10, length - 1);
            var maxValue = (int)Math.Pow(10, length) - 1;
            return _random.Next(minValue, maxValue + 1).ToString().PadLeft(length, '0');
        }
    }

    public static string GenerateAlphanumericCode(int length = 8)
    {
        if (length <= 0 || length > 20)
            throw new ArgumentException("Length must be between 1 and 20", nameof(length));

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        lock (_lockObject)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }

    public static string GenerateSecureCode(int length = 6)
    {
        if (length <= 0 || length > 20)
            throw new ArgumentException("Length must be between 1 and 20", nameof(length));

        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);

        var chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = (char)((bytes[i] % 10) + '0');
        }

        return new string(chars);
    }

    public static string GenerateMixedCode(int length = 8)
    {
        if (length <= 0 || length > 20)
            throw new ArgumentException("Length must be between 1 and 20", nameof(length));

        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = "0123456789";
        
        lock (_lockObject)
        {
            var result = new char[length];
            
            for (int i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    result[i] = letters[_random.Next(letters.Length)];
                }
                else
                {
                    result[i] = numbers[_random.Next(numbers.Length)];
                }
            }

            return new string(result);
        }
    }

    public static string GenerateCustomCode(string allowedChars, int length)
    {
        if (string.IsNullOrEmpty(allowedChars))
            throw new ArgumentException("Allowed characters cannot be null or empty", nameof(allowedChars));
        
        if (length <= 0 || length > 50)
            throw new ArgumentException("Length must be between 1 and 50", nameof(length));

        lock (_lockObject)
        {
            return new string(Enumerable.Repeat(allowedChars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
