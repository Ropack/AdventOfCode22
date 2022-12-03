//var input = "inputTest.txt";
var input = "input.txt";

var lines = File.ReadAllLines(input);

var sum = 0;
foreach (var line in lines)
{
    var halfLength = line.Length / 2;
    var first = line[..halfLength];
    var second = line[halfLength..];

    var intersect = first.ToCharArray().Intersect(second.ToCharArray()).Single();

    var priority = GetPriority(intersect);
    sum += priority;
}

// part 1
Console.WriteLine(sum);

var sum2 = 0;
for (int i = 0; i < lines.Length; i += 3)
{
    var line1 = lines[i];
    var line2 = lines[i + 1];
    var line3 = lines[i + 2];

    var intersect = line1.ToCharArray().Intersect(line2.ToCharArray()).Intersect(line3.ToCharArray()).Single();
    var priority = GetPriority(intersect);
    sum2 += priority;
}

Console.WriteLine(sum2);

int GetPriority(char c)
{
    return c > 96 ? c - 96 : c - 38;
}