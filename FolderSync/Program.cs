using FolderSync.Services;


if (!InputValidator.Validate(args, out var settings)) return;

var logger = new Logger(settings.LogFilePath);
var syncService = new SyncService(logger);
logger.Log("Folder synchronization started.");

while (true)
{
    try
    {
        syncService.SyncFolders(settings.SourcePath, settings.ReplicaPath);
    }
    catch (Exception ex)
    {
        logger.Log($"Synchronization failed: {ex.Message}");
    }

    await Task.Delay(TimeSpan.FromSeconds(settings.IntervalSeconds));
}


