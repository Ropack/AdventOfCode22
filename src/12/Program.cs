var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var sizeX = lines[0].Length;
var sizeY = lines.Length;
var map = new int[sizeX, sizeY];
var startX = 0;
var startY = 0;

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
        }
        else
        {
            map[j, i] = line[j] - 96;
        }
    }
}

// part 1
var result = GetShortestPath(startX, startY);
Console.WriteLine(result);

var list = new List<int>();
for (var x = 0; x < map.GetLength(0); x++)
{
    for (var y = 0; y < map.GetLength(1); y++)
    {
        var position = map[x, y];
        if (position == 1)
        {
            list.Add(GetShortestPath(x, y));
        }
    }
}

// part 2
Console.WriteLine(list.Min());

int GetShortestPath(int startX, int startY)
{
    bool IsEnd(int x, int y) => map[x, y] == 27;
    bool IsInBounds(int x, int y) => x < sizeX && y < sizeY && x >= 0 && y >= 0;

    var visited = new bool[sizeX, sizeY];
    visited[startX, startY] = true;

    var q = new Queue<(int x, int y, int currentLength)>();
    q.Enqueue((startX, startY, 0));

    int? Search(int x, int y, int current, int currentLength)
    {
        if (IsInBounds(x, y) && current > map[x, y] - 2 && !visited[x, y])
        {
            visited[x, y] = true;
            var sum = currentLength + 1;
            if (IsEnd(x, y))
            {
                return sum;
            }

            q.Enqueue((x, y, sum));
        }

        return null;
    }

    while (q.TryDequeue(out var item))
    {
        var x = item.x;
        var y = item.y;
        var current = map[x, y];

        //right
        {
            var newX = x + 1;
            var newY = y;
            var found = Search(newX, newY, current, item.currentLength);
            if (found != null)
            {
                return found.Value;
            }
        }

        // left
        {
            var newX = x - 1;
            var newY = y;
            var found = Search(newX, newY, current, item.currentLength);
            if (found != null)
            {
                return found.Value;
            }
        }

        // bottom
        {
            var newX = x;
            var newY = y + 1;
            var found = Search(newX, newY, current, item.currentLength);
            if (found != null)
            {
                return found.Value;
            }
        }

        // up
        {
            var newX = x;
            var newY = y - 1;
            var found = Search(newX, newY, current, item.currentLength);
            if (found != null)
            {
                return found.Value;
            }
        }
    }

    return int.MaxValue;
}