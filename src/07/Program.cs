using System.Diagnostics;

var input = "input.txt";
var lines = File.ReadAllLines(input);


var currentDirectory = new Directory()
{
    Name = "/"
};
var rootDirectory = currentDirectory;
for (int i = 1; i < lines.Length; i++)
{
    var line = lines[i];
    if (line.StartsWith("$"))
    {
        var command = line.Split(" ")[1];
        if (command == "cd")
        {
            var dirName = line.Split(" ")[2];
            if (dirName == "..")
            {
                currentDirectory = currentDirectory.ParentDirectory;
            }
            else
            {
                var d = currentDirectory.Directories.Single(x => x.Name == dirName);
                currentDirectory = d;
            }
        }
        //else if (command == "ls")
        //{
        //    for (; !line.StartsWith("$"); i++)
        //    {
        //        line = lines[i];
        //        var first = line.Split(" ")[0];
        //        var second = line.Split(" ")[1];
        //        if (first == "dir")
        //        {
        //            var d = new Directory()
        //            {
        //                Name = second,
        //                ParentDirectory = currentDirectory
        //            };
        //        }
        //        else
        //        {
        //            var f = new MyFile()
        //            {
        //                Name = second,
        //                Size = long.Parse(first)
        //            };
        //            currentDirectory.Files.Add(f);
        //        }
        //    }
        //}
    }
    else
    {
        var first = line.Split(" ")[0];
        var second = line.Split(" ")[1];
        if (first == "dir")
        {
            var d = new Directory()
            {
                Name = second,
                ParentDirectory = currentDirectory
            };
            currentDirectory.Directories.Add(d);
        }
        else
        {
            var f = new MyFile()
            {
                Name = second,
                Size = long.Parse(first)
            };
            currentDirectory.Files.Add(f);
        }
    }
}


var allDirectories = new List<Directory>()
{
    rootDirectory
};

void GetDirectories(Directory d)
{
    allDirectories.AddRange(d.Directories);
    foreach (var child in d.Directories)
    {
        GetDirectories(child);
    }
}

GetDirectories(rootDirectory);
var part1 = allDirectories.Where(directory => directory.Size < 100_000).Sum(x => x.Size);
Console.WriteLine(part1);

var spaceNeeded = rootDirectory.Size - 40_000_000;
var minBy = allDirectories.Where(x=>x.Size > spaceNeeded).MinBy(x=> x.Size - spaceNeeded);
Console.WriteLine(minBy.Size);

[DebuggerDisplay("{Name}")]
class Directory
{
    public List<Directory> Directories { get; set; } = new();
    public List<MyFile> Files { get; set; } = new();
    public string Name { get; set; }
    public long Size => Files.Sum(x => x.Size) + Directories.Sum(x => x.Size);
    public Directory? ParentDirectory { get; set; }
}

class MyFile
{
    public string Name { get; set; }
    public long Size { get; set; }
}