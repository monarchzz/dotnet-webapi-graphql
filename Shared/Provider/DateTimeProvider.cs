namespace Shared.Provider;

public static class DateTimeProvider
{
    // public static DateTime VnNow => DateTime.UtcNow.AddHours(7);

    public static DateTime UtcNow => DateTime.UtcNow;

    public static DateTime MaxDate => DateTime.MaxValue;

    public static DateTime MinDate => DateTime.MinValue;
}