namespace FolderSync.Services;

public class Logger
{
    private readonly string _logPath;
    public Logger(string logPath)
    {

        _logPath = logPath;

        
        if (!File.Exists(logPath))
        {
            string? directory = Path.GetDirectoryName(logPath);

            var dirCreated = false;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                dirCreated = true;
            }
            File.WriteAllText(logPath, string.Empty);
            if (dirCreated) Log("Directory for Log created");
            Log("File for Log created");
        }
    }

    public void Log(string message)
    {
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

        File.AppendAllText(_logPath, logMessage + Environment.NewLine);

        Console.WriteLine(logMessage);
    }
}
