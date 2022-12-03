var lines = File.ReadAllLines("input.txt");
List<int> sums = new()
{
    0
};
int index = 0;
foreach (var line in lines)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        sums.Add(0);
        index++;
        continue;
    }

    sums[index] += int.Parse(line);
}

Console.WriteLine(sums.Max());
var sum = sums.OrderByDescending(x=>x).Take(3).Sum();
Console.WriteLine(sum);