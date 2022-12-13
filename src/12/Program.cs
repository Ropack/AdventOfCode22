using System.Diagnostics;

var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var sizeX = lines[0].Length;
var sizeY = lines.Length;
var endX = 0;
var endY = 0;
var map = new int[sizeX, sizeY];
var startX = 0;
var startY = 0;

var visited = new int[sizeX, sizeY];
for (int i = 0; i < sizeY; i++)
{
    var line = lines[i];
    for (int j = 0; j < sizeX; j++)
    {
        if (line[j] == 'S')
        {
            map[j, i] = 0;
            startX = j;
            startY = i;
        }
        else if (line[j] == 'E')
        {
            map[j, i] = 27;
            endX = j;
            endY = i;
        }
        else
        {
            map[j, i] = line[j] - 96;
        }

        visited[j, i] = int.MaxValue;
    }
}

var stopwatch = new Stopwatch();
stopwatch.Start();
//var result = DfsWithStack(visited);
//var result = DfsWithRecursion(visited);
var result = Bfs(startX, startY);
stopwatch.Stop();
Console.WriteLine($"Result {result.result} expanded nodes {result.numberOfExpandedNodes} time {stopwatch.Elapsed}");

var list = new List<int>();
for (var x = 0; x < map.GetLength(0); x++)
{
    for (var y = 0; y < map.GetLength(1); y++)
    {
        var position = map[x, y];
        if (position == 1)
        {
            list.Add(Bfs(x, y).result);
        }
    }
}

// part 2
Console.WriteLine(list.Min());

(int result, int numberOfExpandedNodes) DfsWithStack(int[,] visited1)
{
    var numberOfExpandedNodes = 0;
    var stack = new Stack<(int x, int y, int currentLength)>();
    stack.Push((startX, startY, 0));
    while (stack.TryPop(out var item))
    {
        numberOfExpandedNodes++;
        var x = item.x;
        var y = item.y;
        var currentLength = item.currentLength;
        var current = map[x, y];

        visited1[x, y] = currentLength;
        if (map[x, y] == 27)
        {
            Logger.Log($"Found end, returning {currentLength}");
            continue;
        }

        {
            //right
            var newX = x + 1;
            var newY = y;
            if (x < sizeX - 1 && current > map[newX, newY] - 2 && visited1[newX, newY] > currentLength + 1)
            {
                stack.Push((newX, newY, currentLength + 1));
            }
        }
        {
            // left
            var newX = x - 1;
            var newY = y;
            if (x > 0 && current > map[newX, newY] - 2 && visited1[newX, newY] > currentLength + 1)
            {
                stack.Push((newX, newY, currentLength + 1));
            }
        }
        {
            // bottom
            var newX = x;
            var newY = y + 1;
            if (y < sizeY - 1 && current > map[newX, newY] - 2 && visited1[newX, newY] > currentLength + 1)
            {
                stack.Push((newX, newY, currentLength + 1));
            }
        }
        {
            // up
            var newX = x;
            var newY = y - 1;
            if (y > 0 && current > map[newX, newY] - 2 && visited1[newX, newY] > currentLength + 1)
            {
                stack.Push((newX, newY, currentLength + 1));
            }
        }
    }

    return (visited1[endX, endY], numberOfExpandedNodes);
}

(int result, int numberOfExpandedNodes) DfsWithRecursion(int[,] visited)
{
    var numberOfExpandedNodes = 0;
    F(startX, startY, 0);
    return (visited[endX, endY], numberOfExpandedNodes);

    int F(int x, int y, int currentLength)
    {
        numberOfExpandedNodes++;
        var current = map[x, y];
        visited[x, y] = currentLength;
        if (map[x, y] == 27)
        {
            Logger.Log($"Found end, returning {currentLength}");
            return currentLength;
        }

        {
            // up
            var newX = x;
            var newY = y - 1;
            if (y > 0 && current > map[newX, newY] - 2 && visited[newX, newY] > currentLength + 1)
            {
                F(newX, newY, currentLength + 1);
            }
        }
        {
            // bottom
            var newX = x;
            var newY = y + 1;
            if (y < sizeY - 1 && current > map[newX, newY] - 2 && visited[newX, newY] > currentLength + 1)
            {
                F(newX, newY, currentLength + 1);
            }
        }
        {
            // left
            var newX = x - 1;
            var newY = y;
            if (x > 0 && current > map[newX, newY] - 2 && visited[newX, newY] > currentLength + 1)
            {
                F(newX, newY, currentLength + 1);
            }
        }
        {
            //right
            var newX = x + 1;
            var newY = y;
            if (x < sizeX - 1 && current > map[newX, newY] - 2 && visited[newX, newY] > currentLength + 1)
            {
                F(newX, newY, currentLength + 1);
            }
        }


        return 0;
    }
}

(int result, int numberOfExpandedNodes) Bfs(int startX, int startY)
{
    bool IsEnd(int x, int y) => map[x, y] == 27;
    bool IsInBounds(int x, int y) => x < sizeX && y < sizeY && x >= 0 && y >= 0;

    var visited = new bool[sizeX, sizeY];

    var q = new Queue<(int x, int y, int currentLength)>();
    q.Enqueue((startX, startY, 0));

    void CheckAndEnqueue(int x, int y, int current, int currentLength)
    {
        if (IsInBounds(x, y) && current > map[x, y] - 2 && !visited[x, y])
        {
            q.Enqueue((x, y, currentLength + 1));
        }
    }

    var numberOfExpandedNodes = 0;
    while (q.TryDequeue(out var item))
    {
        numberOfExpandedNodes++;
        var x = item.x;
        var y = item.y;
        var current = map[x, y];
        var currentLength = item.currentLength;

        if (visited[x, y])
        {
            continue;
        }
        visited[x, y] = true;
        if (IsEnd(x, y))
        {
            return (currentLength, numberOfExpandedNodes);
        }

        //right
        {
            var newX = x + 1;
            var newY = y;
            CheckAndEnqueue(newX, newY, current, item.currentLength);
        }

        // left
        {
            var newX = x - 1;
            var newY = y;
            CheckAndEnqueue(newX, newY, current, item.currentLength);
        }

        // bottom
        {
            var newX = x;
            var newY = y + 1;
            CheckAndEnqueue(newX, newY, current, item.currentLength);
        }

        // up
        {
            var newX = x;
            var newY = y - 1;
            CheckAndEnqueue(newX, newY, current, item.currentLength);
        }
    }

    return (int.MaxValue, numberOfExpandedNodes);
}

static class Logger
{
    public static void Log(string s)
    {
        //Console.WriteLine(s);
    }
}