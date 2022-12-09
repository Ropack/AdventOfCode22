var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

//var hPos = (x: 0, y: 0);
//var tPos = (x: 0, y: 0);
//var knotsCount = 2;
var knotsCount = 10;
var knots = new List<(int x, int y)>();
for (int i = 0; i < knotsCount; i++)
{
    knots.Add((0, 0));
}
var visitedPositions = new HashSet<(int, int)> { knots[^1] };

var index = 0;
foreach (var line in lines)
{
    var first = line.Split(" ")[0];
    var second = int.Parse(line.Split(" ")[1]);

    var xAdd = 0;
    var yAdd = 0;
    switch (first)
    {
        case "L":
            xAdd = -1;
            break;
        case "R":
            xAdd = 1;
            break;
        case "U":
            yAdd = 1;
            break;
        case "D":
            yAdd = -1;
            break;
        default:
            throw new Exception();
    }

    for (int i = 0; i < second; i++)
    {
        knots[0] = (knots[0].x + xAdd, knots[0].y + yAdd);
        for (int j = 1; j < knots.Count; j++)
        {
            knots[j] = MoveKnotIfNeeded(knots[j - 1], knots[j]);
        }
        //hPos = (hPos.x + xAdd, hPos.y + yAdd);
        //MoveKnotIfNeeded(hPos, tPos);
        visitedPositions.Add(knots[^1]);
    }

    index++;
}

Console.WriteLine(visitedPositions.Count);

(int x, int y) MoveKnotIfNeeded((int x, int y) previousKnot, (int x, int y) followingKnot)
{
    var x = followingKnot.x;
    var y = followingKnot.y;
    // H is on the right
    if (previousKnot.x - followingKnot.x > 1)
    {
        x = previousKnot.x - 1;
        if (previousKnot.y > followingKnot.y)
        {
            y = followingKnot.y + 1;
        }
        else if (previousKnot.y < followingKnot.y)
        {
            y = followingKnot.y - 1;
        }
        //return (previousKnot.x - 1, previousKnot.y);
    }

    // H is on the left
    if (previousKnot.x - followingKnot.x < -1)
    {
        x = previousKnot.x + 1;
        if (previousKnot.y > followingKnot.y)
        {
            y = followingKnot.y + 1;
        }
        else if (previousKnot.y < followingKnot.y)
        {
            y = followingKnot.y - 1;
        }
        //return (previousKnot.x + 1, previousKnot.y);
    }

    // H is on the top
    if (previousKnot.y - followingKnot.y > 1)
    {
        y = previousKnot.y - 1;
        if (previousKnot.x > followingKnot.x)
        {
            x = followingKnot.x + 1;
        }
        else if (previousKnot.x < followingKnot.x)
        {
            x = followingKnot.x - 1;
        }
        //return (previousKnot.x, previousKnot.y - 1);
    }

    // H is on the bottom
    if (previousKnot.y - followingKnot.y < -1)
    {
        y = previousKnot.y + 1;
        if (previousKnot.x > followingKnot.x)
        {
            x = followingKnot.x + 1;
        }
        else if (previousKnot.x < followingKnot.x)
        {
            x = followingKnot.x - 1;
        }
        //return (previousKnot.x, previousKnot.y + 1);
    }

    return (x, y);
}