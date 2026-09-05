namespace FolderSync.Models;

public class SyncSettings
{
    public string? SourcePath {  get; set; }
    public string? ReplicaPath { get; set; }
    public string? LogFilePath { get; set; }
    public int IntervalSeconds { get; set; } = 0;
}
