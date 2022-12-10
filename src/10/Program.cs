var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var cycle = 1;
var registerX = 1;
var sum = 0;
foreach (var line in lines)
{
    CheckSignificantCycles();
    Draw();
    if (line == "noop")
    {

    }
    else
    {
        var number = int.Parse(line.Split(" ")[1]);
        cycle++;
        CheckSignificantCycles();
        Draw();
        registerX += number;
    }

    cycle++;
}

Console.WriteLine();
Console.WriteLine(sum);

void CheckSignificantCycles()
{
    if (cycle is 20 or 60 or 100 or 140 or 180 or 220)
    {
        sum += registerX * cycle;
    }
}

void Draw()
{
    var pos1 = registerX - 1;
    var pos2 = registerX ;
    var pos3 = registerX +1;
    var renderPos = (cycle-1) % 40;
    if (renderPos == 0)
    {
        Console.WriteLine();
    }

    if (renderPos == pos1 || renderPos == pos2 || renderPos == pos3)
    {
        Console.Write("#");
    }
    else
    {
        Console.Write(".");
    }
};