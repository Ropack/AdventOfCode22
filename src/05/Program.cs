var input = "input.txt";
var lines = File.ReadAllLines(input);

var stackCount = (lines[0].Length + 1) / 4;
var positions = new List<Stack<char>>(Enumerable.Range(0, stackCount).Select(x => new Stack<char>()));
var initialSetupLength = 0;
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
    {
        break;
    }

    initialSetupLength++;
}

for (int i = initialSetupLength - 2; i >= 0; i--)
{
    var line = lines[i];
    for (int j = 0; j < (line.Length + 1); j += 4)
    {
        var crate = line[(j + 1)..(j + 2)][0];
        var index = j / 4;
        if (crate != ' ')
        {
            positions[index].Push(crate);
        }
    }
}

foreach (var line in lines.Skip(initialSetupLength + 1))
{
    var amount = int.Parse(line.Split(" ")[1]);
    var from = int.Parse(line.Split(" ")[3]);
    var to = int.Parse(line.Split(" ")[5]);

    // PART 1
    //for (int i = 0; i < amount; i++)
    //{
    //    var crate = positions[from-1].Pop();
    //    positions[to-1].Push(crate);
    //}

    //PART 2
    var tempStack = new Stack<char>();
    for (int i = 0; i < amount; i++)
    {
        var crate = positions[from - 1].Pop();
        tempStack.Push(crate);
    }

    foreach (var c in tempStack)
    {
        positions[to-1].Push(c);
    }
}

var result = "";
foreach (var position in positions)
{
    result += position.Peek();
}

Console.WriteLine(result);