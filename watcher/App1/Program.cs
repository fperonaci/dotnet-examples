// In principle it monitors the folder and whenever a file with txt
// extension is added (also overwrite) it triggers OnChanged method
// Problem is that sometimes the event is triggered twice

// https://stackoverflow.com/questions/11072295/
// https://stackoverflow.com/questions/1764809/
// https://asp-blogs.azurewebsites.net/ashben/31773
// https://failingfast.io/a-robust-solution-for-filesystemwatcher-firing-events-multiple-times/
// https://github.com/benbhall/FileSystemWatcherMemoryCache
// https://devblogs.microsoft.com/oldnewthing/20140507-00/?p=1053

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