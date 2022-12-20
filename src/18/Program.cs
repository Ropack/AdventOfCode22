using Common;

var input = "input.txt";
//var input = "test-input.txt";
//var input = "test-input2.txt";
var lines = File.ReadAllLines(input);

var size = 22;
var map = new int[size, size, size];

foreach (var line in lines)
{
    var x = int.Parse(line.Split(",")[0]);
    var y = int.Parse(line.Split(",")[1]);
    var z = int.Parse(line.Split(",")[2]);

    map[x, y, z] = 1;
}

InitForPart2();

void InitForPart2()
{
    // all empty positions fill with -1 instead of 0
    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            for (int k = 0; k < size; k++)
            {
                if (map[k, j, i] == 0)
                {
                    map[k, j, i] = -1;
                }
            }
        }
    }

    // DFS - all empty positions that are trapped in lava remains -1, others are set to 0
    var stack = new Stack<(int, int, int)>();
    stack.Push((0,0,0));
    while (stack.TryPop(out var item))
    {
        var (x, y, z) = item;
        if (!map.IsInBounds(x, y, z))
        {
            continue;
        }

        if (map[x, y, z] > -1)
        {
            continue;
        }

        map[x, y, z] = 0;
        stack.Push((x - 1, y, z));
        stack.Push((x, y - 1, z));
        stack.Push((x, y, z - 1));
        stack.Push((x + 1, y, z));
        stack.Push((x, y + 1, z));
        stack.Push((x, y, z + 1));
    }
}

var sum = 0;
for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        var isVisible = true;
        for (int k = 0; k < size; k++)
        {
            if (map[k, j, i] == 0)
            {
                isVisible = true;
                continue;
            }

            if (isVisible && map[k, j, i] == 1)
            {
                sum++;
                isVisible = false;
            }
        }
    }
}

for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        var isVisible = true;
        for (int k = size - 1; k >= 0; k--)
        {
            if (map[k, j, i] == 0)
            {
                isVisible = true;
                continue;
            }

            if (isVisible && map[k, j, i] == 1)
            {
                sum++;
                isVisible = false;
            }
        }
    }
}

for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        var isVisible = true;
        for (int k = 0; k < size; k++)
        {
            if (map[j, k, i] == 0)
            {
                isVisible = true;
                continue;
            }

            if (isVisible && map[j, k, i] == 1)
            {
                sum++;
                isVisible = false;
            }
        }
    }
}

for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        var isVisible = true;
        for (int k = size - 1; k >= 0; k--)
        {
            if (map[j, k, i] == 0)
            {
                isVisible = true;
                continue;
            }

            if (isVisible && map[j, k, i] == 1)
            {
                sum++;
                isVisible = false;
            }
        }
    }
}


for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        var isVisible = true;
        for (int k = 0; k < size; k++)
        {
            if (map[j, i, k] == 0)
            {
                isVisible = true;
                continue;
            }

            if (isVisible && map[j, i, k] == 1)
            {
                sum++;
                isVisible = false;
            }
        }
    }
}

for (int i = 0; i < size; i++)
{
    for (int j = 0; j < size; j++)
    {
        var isVisible = true;
        for (int k = size - 1; k >= 0; k--)
        {
            if (map[j, i, k] == 0)
            {
                isVisible = true;
                continue;
            }

            if (isVisible && map[j, i, k] == 1)
            {
                sum++;
                isVisible = false;
            }
        }
    }
}

Console.WriteLine(sum);