using FolderSync.Services;

string? originPath = null;
string? backUpPath = null;
string? logFilePath = null;
int intervalSeconds = 0;

for (int i = 0; i < args.Length; i++)
{
    string option = args[i];

    switch (option)
    {
        case "--source":
        case "-s":
            if (i + 1 >= args.Length)
            {
                Console.WriteLine("Missing value for --origin.");
                return;
            }

            originPath = args[++i];

            if (!Directory.Exists(originPath))
            {
                Console.WriteLine($"Source directory does not exist: {originPath}");
                return;
            }

            break;

        case "--replica":
        case "-r":
            if (i + 1 >= args.Length)
            {
                Console.WriteLine("Missing value for --backup.");
                return;
            }

            backUpPath = args[++i];
            break;

        case "--interval":
        case "-i":
            if (i + 1 >= args.Length)
            {
                Console.WriteLine("Missing value for --interval.");
                return;
            }

            if (!int.TryParse(args[++i], out intervalSeconds))
            {
                Console.WriteLine("Interval must be a number.");
                return;
            }

            if (intervalSeconds <= 0)
            {
                Console.WriteLine("Interval must be a positive number.");
                return;
            }

            break;

        case "--log":
        case "-l":
            if (i + 1 >= args.Length)
            {
                Console.WriteLine("Missing value for --log.");
                return;
            }

            logFilePath = args[++i];
            break;

        case "--help":
        case "-h":
            Console.WriteLine("""
                FolderSync

                Usage:
                  FolderSync --origin <path> --backup <path> --interval <seconds> --log <filePath>

                Options:
                  -s, --source <path>       Source folder that will be synchronized.
                  -r, --replica <path>       Replica folder.
                  -i, --interval <seconds>  Synchronization interval in seconds.
                  -l, --log <filePath>      Path to the log file.
                  -h, --help                Display usage information.

                Example:
                  FolderSync --source "C:\Source" --replica "D:\Replica" --interval 30 --log "C:\Logs\FolderSync.log"

                """);

            return;

        default:
            Console.WriteLine(
                $"Unknown argument '{option}'. Use --help for usage information.");
            return;
    }
}

if (originPath is null)
{
    Console.WriteLine("Missing required argument: --source");
    return;
}

if (backUpPath is null)
{
    Console.WriteLine("Missing required argument: --replica");
    return;
}

if (intervalSeconds <= 0)
{
    Console.WriteLine("Missing required argument: --interval");
    return;
}

if (logFilePath is null)
{
    Console.WriteLine("Missing required argument: --log");
    return;
}

if (!Directory.Exists(backUpPath))
{
    Console.WriteLine("Replica directory does not exist and will be created.");
}


var logger = new Logger(logFilePath);
var syncService = new SyncService(logger);
logger.Log("Folder synchronization started.");

while (true)
{
    syncService.SyncFolders(originPath, backUpPath);
    await Task.Delay(TimeSpan.FromSeconds(intervalSeconds));
}


