namespace ActiveReportsRtfToHtmlConverter.Helpers;

public static class LogHelper
{
    public static void LogWithTimestamp(string message)
    {
        var timestamp = DateTime.Now;
        Console.WriteLine($"{timestamp} - {message}");
    }
}