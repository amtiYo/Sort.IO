using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.FileIO;

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
            ConsoleKey choice = menu.Show();

            switch (choice)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    sorter.Run();
                    break;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    unzipper.Run();
                    break;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    statistics.Run();
                    break;

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    paths.Run();
                    break;

                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    return;

                default:
                    Console.Clear();
                    Console.CursorVisible = true;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n   SortIO\n");
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
        { ".exe", PathsConfig.RecycleBin },
        { ".msi", PathsConfig.RecycleBin },
        { ".bat", PathsConfig.RecycleBin },
        { ".cmd", PathsConfig.RecycleBin },

        { ".iso", PathsConfig.RecycleBin },
        { ".img", PathsConfig.RecycleBin },
        { ".dmg", PathsConfig.RecycleBin },
        { ".mdf", PathsConfig.RecycleBin },
        { ".mds", PathsConfig.RecycleBin },
        { ".cue", PathsConfig.RecycleBin },
        { ".bin", PathsConfig.RecycleBin },
        { ".nrg", PathsConfig.RecycleBin },

        { ".torrent", PathsConfig.RecycleBin }
    };

    public static readonly Dictionary<string, string> UnzipRules = new Dictionary<string, string>()
    {

    };

    public static readonly Dictionary<string, string> ArchivesRules = new Dictionary<string, string>()
    {
        { ".zip", PathsConfig.Archives },
        { ".rar", PathsConfig.Archives },
        { ".7z", PathsConfig.Archives },
        { ".tar", PathsConfig.Archives },
        { ".gz", PathsConfig.Archives },
        { ".bz2", PathsConfig.Archives },
        { ".xz", PathsConfig.Archives },
        { ".tgz", PathsConfig.Archives },
        { ".tbz2", PathsConfig.Archives },
        { ".txz", PathsConfig.Archives },
        { ".tar.gz", PathsConfig.Archives },
        { ".tar.bz2", PathsConfig.Archives },
        { ".tar.xz", PathsConfig.Archives },
        { ".cab", PathsConfig.Archives }
    };
}

class Menu
{
    public ConsoleKey Show()
    {
        Console.Clear();
        Console.CursorVisible = true;
        Console.Title = "SortIO | Menu";

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("   SortIO\n");
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
        Console.Write("   Select option (1-5): ");
        Console.ResetColor();

        return Console.ReadKey(true).Key;
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
        Console.Title = "SortIO | Sorter";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n   SORT FILES\n");
        Console.ResetColor();

        List<string> sourceFolders = new List<string>
        {
            PathsConfig.Downloads,
            PathsConfig.Desktop
        };

        List<string> existingSources = new List<string>();
        foreach (string source in sourceFolders)
        {
            if (Directory.Exists(source))
            {
                existingSources.Add(source);
            }
        }

        if (existingSources.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("   Downloads and Desktop folders not found.");
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Press Enter to return to menu...");
            Console.ResetColor();
            Console.CursorVisible = true;
            Console.ReadLine();
            return;
        }

        List<string> allFiles = new List<string>();
        foreach (string source in existingSources)
        {
            allFiles.AddRange(Directory.GetFiles(source));
        }

        string[] files = allFiles.ToArray();
        List<string> pendingFiles = new List<string>();

        if (files.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   No files in Downloads/Desktop.");
            Console.ResetColor();
        }

        foreach (string file in files)
        {
            string extension = Path.GetExtension(file).ToLower();
            string fileName = Path.GetFileName(file);

            if (RulesConfig.ExecRules.ContainsKey(extension))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"   [WAIT] {fileName} (Pending decision)");
                Console.ResetColor();

                pendingFiles.Add(file);
                continue;
            }

            if (RulesConfig.Rules.ContainsKey(extension))
            {
                string targetFolder = RulesConfig.Rules[extension];
                MoveFile(file, targetFolder);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"   [SKIP] {fileName}");
                Console.ResetColor();
            }
        }

        if (pendingFiles.Count > 0)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"   Found {pendingFiles.Count} executable/installer files.");
            Console.ResetColor();

            Console.WriteLine("   Choose action:");
            Console.WriteLine("   [1] DELETE (Move to RecycleBin)");
            Console.WriteLine("   [2] LEAVE (Do nothing)");

            Console.Write("\n   Selection: ");
            ConsoleKey key = Console.ReadKey().Key;
            Console.WriteLine("\n");

            if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1)
            {
                foreach (var file in pendingFiles)
                {
                    string extension = Path.GetExtension(file).ToLower();

                    if (RulesConfig.ExecRules.ContainsKey(extension))
                    {
                        string targetFolder = RulesConfig.ExecRules[extension];
                        MoveFile(file, targetFolder);
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("   Files skipped. Left in Downloads.");
                Console.ResetColor();
            }
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("   Press Enter to return to menu...");
        Console.ResetColor();
        Console.CursorVisible = true;
        Console.ReadLine();
    }

    private void MoveFile(string originalFile, string folderName)
    {
        string fileName = Path.GetFileName(originalFile);
        string extension = Path.GetExtension(originalFile);

        if (folderName == PathsConfig.RecycleBin)
        {
            try
            {
                MoveToTrash(originalFile, fileName);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   [MOVED] {fileName} -> {folderName}");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   [ERROR] {fileName}: {e.Message}");
                Console.ResetColor();
            }

            return;
        }

        string targetPath = Path.Combine(PathsConfig.Downloads, folderName);

        if (!Directory.Exists(targetPath))
        {
            Directory.CreateDirectory(targetPath);
        }

        string destFile = Path.Combine(targetPath, fileName);

        int count = 1;
        while (File.Exists(destFile))
        {
            string nameNoExt = Path.GetFileNameWithoutExtension(fileName);
            string newName = $"{nameNoExt}_{count}{extension}";
            destFile = Path.Combine(targetPath, newName);
            count++;
        }

        try
        {
            File.Move(originalFile, destFile);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"   [MOVED] {fileName} -> {folderName}");
            Console.ResetColor();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   [ERROR] {fileName}: {e.Message}");
            Console.ResetColor();
        }
    }

    private void MoveToTrash(string originalFile, string fileName)
    {
        if (PathsConfig.IsMac)
        {
            MoveToTrashMac(originalFile, fileName);
            return;
        }

        MoveToTrashWindows(originalFile);
    }

    private void MoveToTrashWindows(string originalFile)
    {
        FileSystem.DeleteFile(
            originalFile,
            UIOption.OnlyErrorDialogs,
            RecycleOption.SendToRecycleBin);
    }

    private void MoveToTrashMac(string originalFile, string fileName)
    {
        string trashPath = PathsConfig.RecycleBin;
        Directory.CreateDirectory(trashPath);

        string destFile = Path.Combine(trashPath, fileName);
        string extension = Path.GetExtension(fileName);
        int count = 1;
        while (File.Exists(destFile))
        {
            string nameNoExt = Path.GetFileNameWithoutExtension(fileName);
            destFile = Path.Combine(trashPath, $"{nameNoExt}_{count}{extension}");
            count++;
        }

        File.Move(originalFile, destFile);
    }
}

class Unziper
{
    public void Run()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.Title = "SortIO | Unzipper";

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
        Console.Title = "SortIO | Statistics";

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

    public void Run()
    {
        Console.Clear();
        Console.CursorVisible = false;
        Console.Title = "SortIO | Paths";

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n   SYSTEM PATHS\n");
        Console.ResetColor();

        WritePath("User Profile", PathsConfig.UserProfile);
        WritePath("Downloads", PathsConfig.Downloads);
        WritePath("Desktop", PathsConfig.Desktop);
        WritePath("Pictures", PathsConfig.Pictures);
        WritePath("Documents", PathsConfig.Documents);
        WritePath("Videos", PathsConfig.Videos);
        WritePath("Music", PathsConfig.Music);
        WritePath("Trash / Bin", PathsConfig.RecycleBin);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("   Press Enter to return to menu...");
        Console.ResetColor();
        Console.CursorVisible = true;
        Console.ReadLine();
    }

    private void WritePath(string label, string path)
    {
        string displayPath = PathsConfig.FormatPath(path);

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($" {label,-12} ");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"{displayPath,-45} ");
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
    public static readonly bool IsMac =
        RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    public static readonly string UserProfile =
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

    public static readonly string Downloads =
        Path.Combine(UserProfile, "Downloads");

    public static readonly string Desktop =
        IsMac ? Path.Combine(UserProfile, "Desktop")
              : Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

    public static readonly string Pictures =
        IsMac ? Path.Combine(UserProfile, "Pictures")
              : Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

    public static readonly string Documents =
        IsMac ? Path.Combine(UserProfile, "Documents")
              : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public static readonly string Videos =
        IsMac ? Path.Combine(UserProfile, "Movies")
              : Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

    public static readonly string Music =
        IsMac ? Path.Combine(UserProfile, "Music")
              : Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

    public static readonly string Archives =
        Path.Combine(Downloads, "Archives");

    public static readonly string RecycleBin =
        IsMac ? Path.Combine(UserProfile, ".Trash")
              : "Recycle Bin";

    public static string FormatPath(string path)
    {
        if (IsMac && path.StartsWith(UserProfile, StringComparison.Ordinal))
        {
            return "~" + path.Substring(UserProfile.Length);
        }

        return path;
    }
}
