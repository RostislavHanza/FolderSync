using FolderSync.Models;


namespace FolderSync.Services;

public class InputValidator
{
    public static bool Validate(string[] args, out SyncSettings settings)
    {
        settings = new SyncSettings();
        bool validInput = true;
        for (int i = 0; i<args.Length; i++)
        {
            string option = args[i];

            switch (option)
            {
                case "--source":
                case "-s":
                    if (i + 1 >= args.Length)
                    {
                        Console.WriteLine("Missing value for --source.");
                        validInput = false;
                        break;
                    }

                    settings.SourcePath = args[++i];

                    if (!Directory.Exists(settings.SourcePath))
                    {
                        Console.WriteLine($"Source directory does not exist: {settings.SourcePath}");
                        validInput = false;
                    }

                    break;

                case "--replica":
                case "-r":
                    if (i + 1 >= args.Length)
                    {
                        Console.WriteLine($"Missing value for --replica.");
                        validInput = false;
                        break;
                    }

                    settings.ReplicaPath = args[++i];
                    break;

                case "--interval":
                case "-i":
                    if (i + 1 >= args.Length)
                    {
                        Console.WriteLine($"Missing value for --interval.");
                        validInput = false;
                        break;
                    }

                    if (!int.TryParse(args[++i], out int interval))
                    {
                        Console.WriteLine("Interval must be a number.");
                        validInput = false;
                    }

                    if (interval <= 0)
                    {
                        Console.WriteLine("Interval must be a positive number.");
                        validInput = false;
                    }
                    settings.IntervalSeconds = interval;
                    break;

                case "--log":
                case "-l":
                    if (i + 1 >= args.Length)
                    {
                        Console.WriteLine("Missing value for --log.");
                        validInput = false;
                        break;
                    }
                    settings.LogFilePath = args[++i];
                    break;

                case "--help":
                case "-h":
                    Console.WriteLine("""
                                FolderSync

                                Usage:
                                  FolderSync --source <path> --replica <path> --interval <seconds> --log <filePath>

                                Options:
                                  -s, --source <path>       Source folder that will be synchronized.
                                  -r, --replica <path>      Replica folder.
                                  -i, --interval <seconds>  Synchronization interval in seconds.
                                  -l, --log <filePath>      Path to the log file.
                                  -h, --help                Display usage information.

                                Example:
                                  FolderSync --source "C:\Source" --replica "D:\Replica" --interval 30 --log "C:\Logs\FolderSync.log"

                                """);
                    break;

                default:
                    Console.WriteLine($"Unknown argument '{option}'. Use --help for usage information.");
                    validInput = false;
                    break;
            }
        }

        if (settings.SourcePath == null)
        {
            Console.WriteLine("Missing required argument: --source");
            validInput = false;
        }

        if (settings.ReplicaPath == null)
        {
            Console.WriteLine("Missing required argument: --replica");
            validInput = false;
        }

        if (settings.IntervalSeconds <= 0)
        {
            Console.WriteLine("Missing required argument: --interval");
            validInput = false;
        }

        if (settings.LogFilePath == null)
        {
            Console.WriteLine("Missing required argument: --log");
            validInput = false;
        }

        if (!Directory.Exists(settings.ReplicaPath))
        {
            Console.WriteLine("Directory for Replica does not exist and will be created.");
        }

        return validInput;
    }
}
