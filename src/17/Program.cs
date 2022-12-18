var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var jet = lines[0];

var map = new List<string>()
{
    "#######"
};

var rocks = new List<string[]>()
{
    new []
    {
        "..@@@@."
    },
    new []
    {
        "...@...",
        "..@@@..",
        "...@..."
    },
    new []
    {
        "..@@@..",
        "....@..",
        "....@..",
    },
    new[]
    {
        "..@....",
        "..@....",
        "..@....",
        "..@....",
    },
    new []
    {
        "..@@...",
        "..@@...",
    }
};

var jetIndex = 0;
var rockIndex = 0;
var rockHeight = 0;
var rockCounter = 0;

while (true)
{
    map.AddRange(new[]
    {
        ".......",
        ".......",
        ".......",
    });

    rockHeight = map.Count;
    map.AddRange(rocks[rockIndex]);

    DrawMap();
    rockCounter++;
    rockIndex++;
    rockIndex %= rocks.Count;

    while (true)
    {
        if (jet[jetIndex] == '<')
        {
            ShiftLeft();
        }

        if (jet[jetIndex] == '>')
        {
            ShiftRight();
        }
        
        jetIndex++;
        jetIndex %= jet.Length;

        DrawMap();
        if (!MoveDown())
        {
            for (int i = rockHeight; i < map.Count; i++)
            {
                map[i] = map[i].Replace("@", "#");
            }

            break;
        }

        DrawMap();
    }

    if (rockCounter == 2022)
    {
        break;
    }

}
DrawMap();
Console.WriteLine(map.Count -1);

void DrawMap()
{
    //Console.Clear();
    //var list = map.ToList();
    //list.Reverse();
    //foreach (var x in list.Take(20))
    //{
    //    Console.WriteLine(x);
    //}
    //Console.WriteLine();

    //Thread.Sleep(200);
}

bool MoveDown()
{
    var canBeMoved = true;
    for (int i = rockHeight; i < map.Count; i++)
    {
        for (int j = 0; j < 7; j++)
        {
            if (map[i][j] == '@' && map[i - 1][j] == '#')
            {
                canBeMoved = false;
                break;
            }
        }
    }

    if (!canBeMoved)
    {
        return false;
    }

    for (int i = rockHeight; i < map.Count; i++)
    {
        var newRow = map[i - 1].ToCharArray();
        for (int j = 0; j < 7; j++)
        {
            if (map[i][j] == '@')
            {
                newRow[j] = map[i][j];
            }
        }

        map[i - 1] = string.Join("", newRow);
        map[i] = map[i].Replace("@", ".");
    }
    
    rockHeight--;
    if (!map.Last().Contains('#'))
    {
        map.RemoveAt(map.Count - 1);
    }
    return true;
}

void ShiftLeft()
{
    var canBeMoved = true;
    for (int i = rockHeight; i < map.Count; i++)
    {
        if (!canBeMoved)
        {
            break;
        }
        for (int j = 0; j < 7; j++)
        {
            if (map[i][j] == '@')
            {
                if (j == 0)
                {
                    canBeMoved = false;
                    break;
                }

                if (map[i][j - 1] == '#')
                {
                    canBeMoved = false;
                    break;
                }
            }
        }
    }

    if (!canBeMoved)
    {
        return;
    }

    for (int i = rockHeight; i < map.Count; i++)
    {
        var newRow = new char[7];
        for (int j = 0; j < 7; j++)
        {
            newRow[j] = map[i][j];

            if (map[i][j] == '@')
            {
                newRow[j - 1] = '@';
                newRow[j] = '.';
            }
        }

        map[i] = string.Join("", newRow);
    }
}

void ShiftRight()
{
    var canBeMoved = true;
    for (int i = rockHeight; i < map.Count; i++)
    {
        if (!canBeMoved)
        {
            break;
        }
        for (int j = 6; j >= 0; j--)
        {
            if (map[i][j] == '@')
            {
                if (j == 6)
                {
                    canBeMoved = false;
                    break;
                }

                if (map[i][j + 1] == '#')
                {
                    canBeMoved = false;
                    break;
                }
            }
        }
    }

    if (!canBeMoved)
    {
        return;
    }

    for (int i = rockHeight; i < map.Count; i++)
    {
        var newRow = new char[7];
        for (int j = 6; j >= 0; j--)
        {
            newRow[j] = map[i][j];

            if (map[i][j] == '@')
            {
                newRow[j + 1] = '@';
                newRow[j] = '.';
            }
        }

        map[i] = string.Join("", newRow);
    }
}