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

//var targetRockCount = 2022;
var targetRockCount = 1_000_000_000_000;
var jetIndex = 0;
var rockIndex = 0;
var rockHeight = 0;
var rockCounter = 0L;
var milionCounter = 0;
var totalHeight = 0L;
var maxLookaheadLength = 10;
var cacheHit = 0;
var cacheMiss = 0;
int? cachedTerrainHash = null;
var isPruned = false;
(int, int) loopBegin = (0, 0);
var loopList = new List<(int terrainHash, string appliedJet, int droppedRock, string[] terrain, int topTerrainHash, int addedHeight, int rocksCount)>();
var isLoopSkipApplied = false;

var dictionary = new Dictionary<(int terrainHash, int droppedRock), Dictionary<string, (string[] terrain, int terrainHash, int addedHeight, int rocksCount, CounterWrapper counter)>>();
var list = new List<(int terrainHash, string appliedJet, int droppedRock, string[] terrain, int topTerrainHash, int addedHeight)>();

while (true)
{
    if (rockCounter == targetRockCount)
    {
        break;
    }
    if (map.Count > 1_100)
    {
        map.RemoveRange(0, 1_000);
    }

    if (rockCounter % 1_000_000 == 0 && rockCounter != 0)
    {
        milionCounter++;
        Console.WriteLine($"{stopwatch.Elapsed}: {milionCounter} Millions!");
    }

    var terrainHash = cachedTerrainHash ?? GetTerrainHash();
    var appliedJet = "";
    var droppedRock = rockIndex;
    var initialHeight = map.Count;

    if (!isLoopSkipApplied && (terrainHash, droppedRock) == loopBegin)
    {
        var loopHeight = 0;
        var loopRocksCount = 0;
        foreach (var loopItem in loopList)
        {
            loopHeight += loopItem.addedHeight;
            loopRocksCount += loopItem.rocksCount;
        }

        var loopApplyCount = targetRockCount / loopRocksCount - rockCounter / loopRocksCount -1 ;

        rockCounter += loopRocksCount * loopApplyCount;
        totalHeight += loopHeight * loopApplyCount;

        isLoopSkipApplied = true;
    }

    if (!isPruned && rockCounter > 1_000_000)
    {
        foreach (KeyValuePair<(int terrainHash, int droppedRock), Dictionary<string, (string[] terrain, int terrainHash, int addedHeight, int rocksCount, CounterWrapper counter)>> keyValuePair in dictionary)
        {
            var itemsToRemove = keyValuePair.Value.Where(x => x.Value.counter.Counter == 0).Select(x => x.Key);
            foreach (var key in itemsToRemove)
            {
                keyValuePair.Value.Remove(key);
            }
        }

        var keys = dictionary.Where(x => x.Value.Count == 0).Select(x => x.Key);
        foreach (var key in keys)
        {
            dictionary.Remove(key);
        }

        isPruned = true;
        loopBegin = (terrainHash, droppedRock);
    }

    var found = false;
    if (dictionary.TryGetValue((terrainHash, droppedRock), out var innerDictionary))
    {
        var lengths = innerDictionary.Keys
            .Select(x => x.Length)
            .Where(x => x < targetRockCount - rockCounter)
            .OrderByDescending(x => x)
            .ToList();
        //var max = lengths.Max();
        //for (int i = Math.Min(maxLookaheadLength, max); i >= 3; i--)
        foreach (var i in lengths)
        {
            string lookahead;
            (string[] terrain, int terrainHash, int addedHeight, int rocksCount, CounterWrapper counter) value;
            if (isPruned && innerDictionary.Count == 1)
            {
                value = innerDictionary.Single().Value;
                lookahead = innerDictionary.Single().Key;
            }
            else
            {
                if (jetIndex + i > jet.Length)
                {
                    lookahead = jet.Substring(jetIndex, jet.Length - jetIndex)
                                + jet[..(i - (jet.Length - jetIndex))];
                }
                else
                {
                    lookahead = jet.Substring(jetIndex, i);
                }

                if (!innerDictionary.TryGetValue(lookahead, out value)) continue;
            }

            map.AddRange(value.terrain);

            if (!isPruned)
            {
                list.Add((terrainHash, lookahead, droppedRock, value.terrain, value.terrainHash, value.addedHeight));
                if (list.Count > 100)
                {
                    list.RemoveAt(0);
                }
            }
            else
            {
                loopList.Add((terrainHash, lookahead, droppedRock, value.terrain, value.terrainHash, value.addedHeight, value.rocksCount));
            }
            totalHeight += value.addedHeight;
            found = true;
            jetIndex += lookahead.Length;
            jetIndex %= jet.Length;
            rockIndex += value.rocksCount;
            rockIndex %= rocks.Count;
            rockCounter += value.rocksCount;
            cachedTerrainHash = value.terrainHash;
            value.counter.Counter++;

            break;
        }
    }

    if (found)
    {
        //Console.WriteLine("Cache hit");
        cacheHit++;
        continue;
    }

    cacheMiss++;
    cachedTerrainHash = null;

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
    var topTerrainHash = GetTerrainHash();
    if (!dictionary.TryAdd((terrainHash, droppedRock), new Dictionary<string, (string[] terrain, int terrainHash, int addedHeight, int rocksCount, CounterWrapper counter)>()
        {
            { appliedJet, (terrain, topTerrainHash, addedHeight, 1, new CounterWrapper()) }
        }))
    {
        dictionary[(terrainHash, droppedRock)].TryAdd(appliedJet, (terrain, topTerrainHash, addedHeight, 1, new CounterWrapper()));
    }

    // This should have been an optimization, that take larger chunks of jet and add them to cache for later use
    // It somehow works for test input, but does not for real input - results are the bigger the count of list items is taken (in condition of "for")

    //var cumulativeAppliedJet = appliedJet;
    //var cumulativeAddedHeight = addedHeight;
    //var cumulativeRocksCount = 1;
    //for (int i = list.Count - 1; i >= Math.Max(list.Count - 20, 0); i--)
    //{
    //    cumulativeAppliedJet = list[i].appliedJet + cumulativeAppliedJet;
    //    if (cumulativeAppliedJet.Length > jet.Length)
    //    {
    //        break;
    //    }
    //    cumulativeAddedHeight += list[i].addedHeight;
    //    cumulativeRocksCount++;
    //    dictionary[(list[i].terrainHash, list[i].droppedRock)].TryAdd(cumulativeAppliedJet, (terrain, topTerrainHash, cumulativeAddedHeight, cumulativeRocksCount, new CounterWrapper()));
    //    if (cumulativeAppliedJet.Length > maxLookaheadLength)
    //    {
    //        maxLookaheadLength = cumulativeAppliedJet.Length;
    //    }
    //}

    //list.Add((terrainHash, appliedJet, droppedRock, terrain, topTerrainHash, addedHeight));
    //if (list.Count > 100)
    //{
    //    list.RemoveAt(0);
    //}
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

[DebuggerDisplay("{Counter}")]
class CounterWrapper
{
    public int Counter { get; set; }
    public override string ToString()
    {
        return Counter.ToString();
    }
}