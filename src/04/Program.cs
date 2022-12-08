var input = "input.txt";

var lines = File.ReadAllLines(input);

var containingSum = 0;
var overlapingSum = 0;
foreach (var line in lines)
{
    var first = line.Split(",")[0];
    var second = line.Split(",")[1];
    var firstStart = int.Parse(first.Split("-")[0]);
    var firstEnd = int.Parse(first.Split("-")[1]);
    
    var secondStart = int.Parse(second.Split("-")[0]);
    var secondEnd = int.Parse(second.Split("-")[1]);

    if (firstStart >= secondStart && firstEnd <= secondEnd)
    {
        containingSum++;
        overlapingSum++;
    }
    else if(secondStart >= firstStart && secondEnd <= firstEnd)
    {
        containingSum++;
        overlapingSum++;
    }
    else if (firstStart >= secondStart && firstStart <= secondEnd)
    {
        overlapingSum++;
    }
    else if (firstEnd >= secondStart && firstEnd <= secondEnd)
    {
        overlapingSum++;
    }
}

Console.WriteLine(containingSum);
Console.WriteLine(overlapingSum);
