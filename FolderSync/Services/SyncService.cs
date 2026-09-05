using System.Security.Cryptography;

namespace FolderSync.Services;

public class SyncService
{
    private readonly Logger _logger;

    public SyncService(Logger logger)
    {
        _logger = logger;
    }

    public void SyncFolders(string originalPath, string backUpPath)
    {
        List<string> originalFolders = new();
        List<string> backUpFolders = new();

        if (!Directory.Exists(backUpPath))
        {
            Directory.CreateDirectory(backUpPath);
            _logger.Log("Directory created: " + backUpPath);
        }

        backUpFolders.AddRange(Directory.GetDirectories(backUpPath, "*", searchOption: SearchOption.AllDirectories));

        originalFolders.Add(originalPath);
        originalFolders.AddRange(Directory.GetDirectories(originalPath, "*", searchOption: SearchOption.AllDirectories));


        foreach (var dirPath in originalFolders)
        {
            string relativePath = Path.GetRelativePath(originalPath, dirPath);
            string backUpFolderPath = Path.Combine(backUpPath, relativePath);

            CheckSyncState(dirPath, backUpFolderPath);

            backUpFolders.Remove(backUpFolderPath);
        }


        foreach (var dir in backUpFolders.OrderByDescending(x => x.Length))
        {
            Directory.Delete(dir, recursive: true);
            _logger.Log("Directory deleted : " + dir);
        }
    }

    private void CheckSyncState(string originalPath, string backUpPath)
    {
        if (!Directory.Exists(backUpPath))
        {
            Directory.CreateDirectory(backUpPath);
            _logger.Log("Directory created : " + backUpPath);
        }

        var toCopyFiles = Directory.GetFiles(originalPath, "*", searchOption: SearchOption.TopDirectoryOnly);
        List<string> backUpFiles = Directory.GetFiles(backUpPath, "*", searchOption: SearchOption.TopDirectoryOnly).ToList();

        foreach (var file in toCopyFiles)
        {
            var fileName = Path.GetFileName(file);
            string backupFile = Path.Combine(backUpPath, fileName);

            if (!File.Exists(backupFile))
            {
                File.Copy(file, backupFile);
                _logger.Log("File created : " + backupFile);
            }
            else
            {
                var originSize = new FileInfo(file).Length;
                var backupSize = new FileInfo(backupFile).Length;
                if (originSize != backupSize || CalculateHash(file) != CalculateHash(backupFile))
                {
                    File.Copy(file, backupFile, overwrite: true);
                    _logger.Log("File updated : " + backupFile);

                }
            }
            backUpFiles.Remove(backupFile);
        }

        foreach (var file in backUpFiles)
        {
            File.Delete(file);
            _logger.Log("File deleted : " + file);
        }
    }

    private string CalculateHash(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var sha256 = SHA256.Create();

        byte[] hash = sha256.ComputeHash(stream);
        return Convert.ToHexString(hash);
    }
}
