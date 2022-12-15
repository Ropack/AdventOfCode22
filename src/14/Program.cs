var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var map = new string[1000, 1000];
var maxY = int.MinValue;
var maxX = int.MinValue;
var minX = int.MaxValue;

foreach (var line in lines)
{
    var obstacles = line.Split(" -> ");
    for (var index = 1; index < obstacles.Length; index++)
    {
        var obstacle1 = obstacles[index - 1];
        var obstacle2 = obstacles[index];
        var startX = int.Parse(obstacle1.Split(",")[0]);
        var startY = int.Parse(obstacle1.Split(",")[1]);

        var endX = int.Parse(obstacle2.Split(",")[0]);
        var endY = int.Parse(obstacle2.Split(",")[1]);

        UpdateMaxAndMins(startY, endY, startX, endX);

        for (int i = Math.Min(startY, endY); i <= Math.Max(startY, endY); i++)
        {
            for (int j = Math.Min(startX, endX); j <= Math.Max(startX, endX); j++)
            {
                map[j, i] = "#";
            }
        }
    }
}

for (int x = 0; x < map.GetLength(0); x++)
{
    map[x, maxY+2] = "#";
}


var sandCounter = 0;
bool isFallingToAbyss = false;
while (!isFallingToAbyss)
{
    //DrawMap();
    var currentX = 500;
    var currentY = 0;
    var isRested = false;
    do
    {
        isRested = false;

        //if (currentY > maxY)
        //{
        //    isFallingToAbyss = true;
        //    break;
        //}

        if (IsFreeSpace(currentX, currentY + 1))
        {
            currentY++;
            continue;
        }
        if (IsFreeSpace(currentX - 1, currentY + 1))
        {
            currentY++;
            currentX--;
            continue;
        }
        if (IsFreeSpace(currentX + 1, currentY + 1))
        {
            currentY++;
            currentX++;
            continue;
        }

        sandCounter++;
        map[currentX, currentY] = "o";
        isRested = true;

        if (currentX == 500 && currentY == 0)
        {
            isFallingToAbyss = true;
            break;
        }

    } while (!isRested);
}

Console.WriteLine(sandCounter);

bool IsFreeSpace(int x, int y) => map[x, y] != "#" && map[x, y] != "o";

void DrawMap()
{

    for (int y = 0; y <= maxY + 2; y++)
    {
        for (int x = minX - 1; x <= maxX + 1; x++)
        {
            if (map[x, y] == null)
            {
                Console.Write(".");
            }
            else
            {
                Console.Write(map[x, y]);

            }

        }

        Console.WriteLine();
    }
    Console.WriteLine();
}

void UpdateMaxAndMins(int startY1, int endY1, int startX1, int endX1)
{
    if (startY1 > maxY)
    {
        maxY = startY1;
    }

    if (endY1 > maxY)
    {
        maxY = endY1;
    }

    if (startX1 > maxX)
    {
        maxX = startX1;
    }

    if (endX1 > maxX)
    {
        maxX = endX1;
    }

    if (startX1 < minX)
    {
        minX = startX1;
    }

    if (endX1 < minX)
    {
        minX = endX1;
    }
}