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

            if (isVisible)
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

            if (isVisible)
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

            if (isVisible)
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

            if (isVisible)
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

            if (isVisible)
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

            if (isVisible)
            {
                sum++;
                isVisible = false;
            }
        }
    }
}

Console.WriteLine(sum);