namespace Common.Utilities.DateTime;

public static class DateTimeUtilities
{
    public static bool IsWeekend(System.DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }

    public static bool IsBusinessDay(System.DateTime date)
    {
        return !IsWeekend(date);
    }

    public static System.DateTime GetNextBusinessDay(System.DateTime date)
    {
        var nextDay = date.AddDays(1);
        while (IsWeekend(nextDay))
        {
            nextDay = nextDay.AddDays(1);
        }
        return nextDay;
    }

    public static System.DateTime GetPreviousBusinessDay(System.DateTime date)
    {
        var previousDay = date.AddDays(-1);
        while (IsWeekend(previousDay))
        {
            previousDay = previousDay.AddDays(-1);
        }
        return previousDay;
    }

    public static int GetBusinessDaysBetween(System.DateTime startDate, System.DateTime endDate)
    {
        var businessDays = 0;
        var currentDate = startDate.Date;

        while (currentDate <= endDate.Date)
        {
            if (IsBusinessDay(currentDate))
                businessDays++;
            currentDate = currentDate.AddDays(1);
        }

        return businessDays;
    }

    public static System.DateTime GetStartOfWeek(System.DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
    {
        var diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
        return date.AddDays(-1 * diff).Date;
    }

    public static System.DateTime GetEndOfWeek(System.DateTime date, DayOfWeek startOfWeek = DayOfWeek.Monday)
    {
        var start = GetStartOfWeek(date, startOfWeek);
        return start.AddDays(6);
    }

    public static System.DateTime GetStartOfMonth(System.DateTime date)
    {
        return new System.DateTime(date.Year, date.Month, 1);
    }

    public static System.DateTime GetEndOfMonth(System.DateTime date)
    {
        return new System.DateTime(date.Year, date.Month, System.DateTime.DaysInMonth(date.Year, date.Month));
    }

    public static System.DateTime GetStartOfYear(System.DateTime date)
    {
        return new System.DateTime(date.Year, 1, 1);
    }

    public static System.DateTime GetEndOfYear(System.DateTime date)
    {
        return new System.DateTime(date.Year, 12, 31);
    }

    public static int GetAge(System.DateTime birthDate, System.DateTime? referenceDate = null)
    {
        var reference = referenceDate ?? System.DateTime.Today;
        var age = reference.Year - birthDate.Year;
        
        if (birthDate.Date > reference.AddYears(-age))
            age--;

        return age;
    }

    public static bool IsLeapYear(int year)
    {
        return year % 4 == 0 && (year % 100 != 0 || year % 400 == 0);
    }

    public static string GetRelativeTime(System.DateTime date)
    {
        var now = System.DateTime.UtcNow;
        var timeSpan = now - date;

        if (timeSpan.TotalDays >= 365)
        {
            var years = (int)(timeSpan.TotalDays / 365);
            return years == 1 ? "1 year ago" : $"{years} years ago";
        }
        
        if (timeSpan.TotalDays >= 30)
        {
            var months = (int)(timeSpan.TotalDays / 30);
            return months == 1 ? "1 month ago" : $"{months} months ago";
        }
        
        if (timeSpan.TotalDays >= 7)
        {
            var weeks = (int)(timeSpan.TotalDays / 7);
            return weeks == 1 ? "1 week ago" : $"{weeks} weeks ago";
        }
        
        if (timeSpan.TotalDays >= 1)
        {
            var days = (int)timeSpan.TotalDays;
            return days == 1 ? "1 day ago" : $"{days} days ago";
        }
        
        if (timeSpan.TotalHours >= 1)
        {
            var hours = (int)timeSpan.TotalHours;
            return hours == 1 ? "1 hour ago" : $"{hours} hours ago";
        }
        
        if (timeSpan.TotalMinutes >= 1)
        {
            var minutes = (int)timeSpan.TotalMinutes;
            return minutes == 1 ? "1 minute ago" : $"{minutes} minutes ago";
        }
        
        return "Just now";
    }

    public static bool IsExpired(System.DateTime date, System.DateTime? referenceDate = null)
    {
        var reference = referenceDate ?? System.DateTime.UtcNow;
        return date < reference;
    }

    public static bool IsExpired(System.DateTime date, TimeSpan expirationTime, System.DateTime? referenceDate = null)
    {
        var reference = referenceDate ?? System.DateTime.UtcNow;
        var expirationDate = date.Add(expirationTime);
        return expirationDate < reference;
    }

    public static TimeSpan GetTimeUntilExpiration(System.DateTime date, TimeSpan expirationTime, System.DateTime? referenceDate = null)
    {
        var reference = referenceDate ?? System.DateTime.UtcNow;
        var expirationDate = date.Add(expirationTime);
        var remaining = expirationDate - reference;
        
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }
}
