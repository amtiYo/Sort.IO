using System;
using System.Collections.Generic;
using System.IO;

namespace DownloadSort;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Menu menu = new Menu();
        Sorter sorter = new Sorter();
        Unziper unzipper = new Unziper();
        Statistics statistics = new Statistics();
        Paths paths = new Paths();

        while (true)
        {
            string? choice = menu.Show();

            switch (choice)
            {
                case "1":
                    sorter.Run();
                    break;

                case "2":
                    unzipper.Run();
                    break;

                case "3":
                    statistics.Run();
                    break;

                case "4":
                    paths.Run();
                    break;

                case "5":
                    return;
                default:
                    Console.Clear();
                    Console.CursorVisible = true;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n   DOWNLOAD SORTER\n");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   Unknown option.");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n   Press Enter to return to menu...");
                    Console.ResetColor();
                    Console.ReadLine();
                    break;
            }
        }
    }
}

static class RulesConfig
{
    public static readonly Dictionary<string, string> Rules = new Dictionary<string, string>()
    {
            // Pictures
            { ".jpg", PathsConfig.Pictures },
            { ".jpeg", PathsConfig.Pictures },
            { ".png", PathsConfig.Pictures },
            { ".gif", PathsConfig.Pictures },
            { ".bmp", PathsConfig.Pictures },
            { ".webp", PathsConfig.Pictures },
            { ".svg", PathsConfig.Pictures },
            { ".ico", PathsConfig.Pictures },
            { ".tiff", PathsConfig.Pictures },
            { ".heic", PathsConfig.Pictures },
            { ".raw", PathsConfig.Pictures },
            { ".psd", PathsConfig.Pictures },
            { ".ai", PathsConfig.Pictures },

            // Video
            { ".mp4", PathsConfig.Videos },
            { ".mkv", PathsConfig.Videos },
            { ".avi", PathsConfig.Videos },
            { ".mov", PathsConfig.Videos },
            { ".webm", PathsConfig.Videos },
            { ".flv", PathsConfig.Videos },
            { ".wmv", PathsConfig.Videos },
            { ".m4v", PathsConfig.Videos },
            { ".3gp", PathsConfig.Videos },

            // Music
            { ".mp3", PathsConfig.Music },
            { ".wav", PathsConfig.Music },
            { ".flac", PathsConfig.Music },
            { ".ogg", PathsConfig.Music },
            { ".aac", PathsConfig.Music },
            { ".m4a", PathsConfig.Music },
            { ".wma", PathsConfig.Music },
            { ".mid", PathsConfig.Music },
            { ".midi", PathsConfig.Music },

            // Documents
            { ".pdf", PathsConfig.Documents },
            { ".txt", PathsConfig.Documents },
            { ".doc", PathsConfig.Documents },
            { ".docx", PathsConfig.Documents },
            { ".xls", PathsConfig.Documents },
            { ".xlsx", PathsConfig.Documents },
            { ".ppt", PathsConfig.Documents },
            { ".pptx", PathsConfig.Documents },
            { ".rtf", PathsConfig.Documents },
            { ".odt", PathsConfig.Documents },
            { ".csv", PathsConfig.Documents },
            { ".md", PathsConfig.Documents },
            { ".epub", PathsConfig.Documents },
            { ".fb2", PathsConfig.Documents },
    };

    public static readonly Dictionary<string, string> ExecRules = new Dictionary<string, string>()
    {

    };

    public static readonly Dictionary<string, string> UnzipRules = new Dictionary<string, string>()
    {

    };

}

class Menu
{
    public string? Show()
    {
        Console.Clear();
        Console.CursorVisible = true;
        Console.Title = "Download Sorter | Menu";

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("   DOWNLOAD SORTER\n");
        Console.ResetColor();

        WriteSectionHeader("Main");
        WriteOption("1", "Sort files", ConsoleColor.Cyan);
        WriteOption("2", "Unzip & sort", ConsoleColor.Cyan);
        WriteOption("3", "Statistics", ConsoleColor.Cyan);

        Console.WriteLine();

        WriteSectionHeader("Tools");
        WriteOption("4", "Check system paths", ConsoleColor.DarkGray);

        Console.WriteLine();

        WriteSectionHeader("System");
        WriteOption("5", "Exit", ConsoleColor.Red);

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("   Select option (1-5) and press Enter: ");
        Console.ResetColor();
        string? choice = Console.ReadLine();

        return choice;
    }

    private void WriteSectionHeader(string title)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"   {title.ToUpper()}");
        Console.ResetColor();
    }

    private void WriteOption(string key, string text, ConsoleColor keyColor)
    {
        Console.ForegroundColor = keyColor;
        Console.Write($"   {key}) ");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}

class Sorter
{
    public void Run()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.Title = "Download Sorter | Sorter";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n   SORT FILES\n");
        Console.ResetColor();

        if (Directory.Exists(PathsConfig.Downloads))
        {
            string[] files = Directory.GetFiles(PathsConfig.Downloads);

            if (files.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("   No files in Downloads.");
                Console.ResetColor();
            }

            foreach (string file in files)
            {
                string extension = Path.GetExtension(file).ToLower();
                string fileName = Path.GetFileName(file);

                if (RulesConfig.Rules.ContainsKey(extension))
                {
                    string targetFolder = RulesConfig.Rules[extension];
                    string destFile = Path.Combine(targetFolder, fileName);

                    int count = 1;
                    while (File.Exists(destFile))
                    {
                        string nameNoExt = Path.GetFileNameWithoutExtension(fileName);
                        string newName = $"{nameNoExt}_{count}{extension}";
                        destFile = Path.Combine(targetFolder, newName);
                        count++;
                    }
                    try
                    {
                        File.Move(file, destFile);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"   [OK] {fileName} -> {targetFolder}");
                        Console.ResetColor();
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"   [ERROR] {fileName}: {e.Message}");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"   [SKIP] {fileName}");
                    Console.ResetColor();
                }
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("   Downloads folder not found.");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Check your paths in system settings.");
            Console.ResetColor();
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("   Press Enter to return to menu...");
        Console.ResetColor();
        Console.CursorVisible = true;
        Console.ReadLine();
    }
}

class Unziper
{
    public void Run()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.Title = "Download Sorter | Unzipper";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n   UNZIP & SORT\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("   This feature is in progress.");
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("   Press Enter to return to menu...");
        Console.ResetColor();
        Console.CursorVisible = true;
        Console.ReadLine();
    }
}

class Statistics
{
    public void Run()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.Title = "Download Sorter | Statistics";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n   STATISTICS\n");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("   This feature is in progress.");
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("   Press Enter to return to menu...");
        Console.ResetColor();
        Console.CursorVisible = true;
        Console.ReadLine();
    }
}

class Paths
{
    // Class implementation helped by ChatGPT.

    public void Run()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.Title = "Download Sorter | Paths";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n   SYSTEM PATHS\n");
        Console.ResetColor();

        WritePath("User Profile", PathsConfig.UserProfile);
        WritePath("Downloads",    PathsConfig.Downloads);
        WritePath("Pictures",     PathsConfig.Pictures);
        WritePath("Documents",    PathsConfig.Documents);
        WritePath("Videos",       PathsConfig.Videos);
        WritePath("Music",        PathsConfig.Music);
        WritePath("Trash / Bin",  PathsConfig.RecycleBin);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("   Press Enter to return to menu...");
        Console.ResetColor();
        Console.CursorVisible = true;
        Console.ReadLine();
    }

    private void WritePath(string label, string path)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($" {label,-12} ");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"{path,-45} ");
        Console.ResetColor();

        if (path == PathsConfig.RecycleBin)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[System]");
        }
        else if (Directory.Exists(path))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[OK]");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Missing]");
        }

        Console.ResetColor();
    }
}

static class PathsConfig
{
    public static readonly string UserProfile =
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public static readonly string Downloads =
        Path.Combine(UserProfile, "Downloads");

    public static readonly string Pictures =
        Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

    public static readonly string Documents =
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public static readonly string Videos =
        Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

    public static readonly string Music =
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

    public static readonly string RecycleBin =
        "::RECYCLE_BIN::";
}
