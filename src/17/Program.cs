using System.Diagnostics;

var input = "input.txt";

//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var stopwatch = new Stopwatch();
stopwatch.Start();

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
var milionCounter = 0;
var totalHeight = 0L;

var dictionary = new Dictionary<(int, string, int), (string[], int)>();

while (true)
{
    if (rockCounter == 1_000_000_000)
    {
        break;
    }
    if (map.Count > 1_100)
    {
        map.RemoveRange(0, 1_000);
    }

    if (rockCounter % 1_000_000 == 0)
    {
        milionCounter++;
        Console.WriteLine($"{stopwatch.Elapsed}: {milionCounter} Millions!");
    }

    var terrainHash = GetTerrainHash();
    //var previousTerrain = GetTopTerrain();
    var appliedJet = "";
    var droppedRock = rockIndex;
    var initialHeight = map.Count;

    var found = false;
    for (int i = 3; i < 50; i++)
    {
        string lookahead;
        if (jetIndex + i > jet.Length)
        {
            lookahead = jet.Substring(jetIndex, jet.Length - jetIndex)
                + jet[..(i - (jet.Length - jetIndex))];
        }
        else
        {
            lookahead = jet.Substring(jetIndex, i);
        }

        if (dictionary.TryGetValue((terrainHash, lookahead, droppedRock), out var value))
        {
            map.AddRange(value.Item1);
            totalHeight += value.Item2;
            found = true;
            jetIndex += lookahead.Length;
            jetIndex %= jet.Length;
            rockIndex++;
            rockIndex %= rocks.Count;
            rockCounter++;
            break;
        }
    }

    if (found)
    {
        //Console.WriteLine("Cache hit");
        continue;
    }

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

        appliedJet += jet[jetIndex];

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

    var addedHeight = map.Count - initialHeight;
    totalHeight += addedHeight;


    var terrain = GetTopTerrain();
    dictionary.Add((terrainHash, appliedJet, droppedRock), (terrain, addedHeight));
}

DrawMap();
Console.WriteLine(totalHeight);

stopwatch.Stop();
Console.WriteLine(stopwatch.Elapsed);

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

int GetTerrainHash()
{
    var terrain = new int[7];
    for (int i = 0; i < 7; i++)
    {
        var depth = 0;
        while (map[^(depth + 1)][i] != '#')
        {
            depth++;
        }

        terrain[i] = depth;
    }

    return (terrain[0], terrain[1], terrain[2], terrain[3], terrain[4], terrain[5], terrain[6]).GetHashCode();
}

string[] GetTopTerrain()
{
    var terrain = new int[7];
    for (int i = 0; i < 7; i++)
    {
        var depth = 0;
        while (map[^(depth + 1)][i] != '#')
        {
            depth++;
        }

        terrain[i] = depth;
    }

    var max = terrain.Max();
    var result = new string[max + 1];
    map.CopyTo(map.Count - max - 1, result, 0, max + 1);
    return result;
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