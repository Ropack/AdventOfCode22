var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var sum = 0;
var maxScenicScore = 0;
for (int i = 0; i < lines.Length; i++)
{
    var line = lines[i];
    for (int j = 0; j < line.Length; j++)
    {
        var height = int.Parse(line.Substring(j, 1));
        if (IsVisible(j, i, height))
        {
            sum++;
        }

        var score = GetScenicScore(j, i, height);
        if (score > maxScenicScore)
        {
            maxScenicScore = score;
        }
    }
}

Console.WriteLine(sum);
Console.WriteLine(maxScenicScore);


bool IsVisible(int x, int y, int checkedHeight)
{
    // from left
    var isVisibleFromLeft = true;
    for (int i = 0; i < x; i++)
    {
        var height = int.Parse(lines[y].Substring(i, 1));
        if (height >= checkedHeight)
        {
            isVisibleFromLeft = false;
            break;
        }
    }

    // from right
    var isVisibleFromRight = true;
    for (int i = x + 1; i < lines[y].Length; i++)
    {
        var height = int.Parse(lines[y].Substring(i, 1));
        if (height >= checkedHeight)
        {
            isVisibleFromRight = false;
            break;
        }
    }

    // from top
    var isVisibleFromTop = true;
    for (int i = 0; i < y; i++)
    {
        var height = int.Parse(lines[i].Substring(x, 1));
        if (height >= checkedHeight)
        {
            isVisibleFromTop = false;
            break;
        }
    }

    // from bottom
    var isVisibleFromBottom = true;
    for (int i = y + 1; i < lines.Length; i++)
    {
        var height = int.Parse(lines[i].Substring(x, 1));
        if (height >= checkedHeight)
        {
            isVisibleFromBottom = false;
            break;
        }
    }

    return isVisibleFromLeft || isVisibleFromRight || isVisibleFromTop || isVisibleFromBottom;
}

int GetScenicScore(int x, int y, int checkedHeight)
{
    // from left
    var scoreFromLeft = 0;
    for (int i = x - 1; i >= 0; i--)
    {
        var height = int.Parse(lines[y].Substring(i, 1));
        scoreFromLeft++;
        if (height >= checkedHeight)
        {
            break;
        }
    }

    // from right
    var scoreFromRight = 0;
    for (int i = x + 1; i < lines[y].Length; i++)
    {
        var height = int.Parse(lines[y].Substring(i, 1));
        scoreFromRight++;
        if (height >= checkedHeight)
        {
            break;
        }
    }

    // from top
    var scoreFromTop = 0;
    for (int i = y - 1; i >= 0; i--)
    {
        var height = int.Parse(lines[i].Substring(x, 1));
        scoreFromTop++;
        if (height >= checkedHeight)
        {
            break;
        }
    }

    // from bottom
    var scoreFromBottom = 0;
    for (int i = y + 1; i < lines.Length; i++)
    {
        var height = int.Parse(lines[i].Substring(x, 1));
        scoreFromBottom++;
        if (height >= checkedHeight)
        {
            break;
        }
    }

    return scoreFromLeft * scoreFromRight * scoreFromTop * scoreFromBottom;
}