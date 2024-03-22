// In principle it monitors the folder and whenever a file with txt
// extension is added (also overwrite) it triggers OnChanged method
// Problem is that sometimes the event is triggered twice

var path = "/home/fp/Desktop/watcher/data";

using var watcher = new FileSystemWatcher(path);

watcher.NotifyFilter = NotifyFilters.LastWrite;
watcher.Filter = "*.txt";
watcher.IncludeSubdirectories = true;
watcher.EnableRaisingEvents = true;
watcher.Changed += OnChanged;

Console.WriteLine("Press enter to exit.");
Console.ReadLine();

static void OnChanged(object sender, FileSystemEventArgs e)
{
    if (e.ChangeType != WatcherChangeTypes.Changed)
    {
        return;
    }
    Console.WriteLine($"Changed: {e.FullPath}");

    using var reader = new StreamReader(e.FullPath);
    Console.Write(reader.ReadToEnd());
}