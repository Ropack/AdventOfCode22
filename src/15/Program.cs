var input = "input.txt";
var targetY = 2000000;
var size = 4000001;
//var input = "test-input.txt";
//var targetY = 10;
//var size = 21;
var lines = File.ReadAllLines(input);

var dictionary = new Dictionary<int, int>();
var beaconPositions = new HashSet<(int, int)>();
var rows = new Dictionary<int, int>[size];
for (var index = 0; index < rows.Length; index++)
{
    rows[index] = new Dictionary<int, int>();
}

foreach (var line in lines)
{
    var sensorX = int.Parse(line.Split(' ')[2][2..^1]);
    var sensorY = int.Parse(line.Split(' ')[3][2..^1]);
    var beaconX = int.Parse(line.Split(' ')[8][2..^1]);
    var beaconY = int.Parse(line.Split(' ')[9][2..]);

    beaconPositions.Add((beaconX, beaconY));

    var distance = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);



    {
        var distanceToTargetY = Math.Abs(sensorY - targetY);
        if (distanceToTargetY <= distance)
        {
            var remainingDistance = distance - distanceToTargetY;
            var beginX = sensorX - remainingDistance;
            var endX = sensorX + remainingDistance;
            if (dictionary.ContainsKey(beginX))
            {
                dictionary[beginX] += 1;
            }
            else
            {
                dictionary.Add(beginX, 1);
            }

            if (dictionary.ContainsKey(endX + 1))
            {
                dictionary[endX + 1] -= 1;
            }
            else
            {
                dictionary.Add(endX + 1, -1);
            }
        }
    }

    for (int i = Math.Max(sensorY - distance, 0); i < Math.Min(sensorY + distance + 1, size); i++)
    {
        var distanceToY = Math.Abs(sensorY - i);
        var remainingDistance = distance - distanceToY;
        var beginX = sensorX - remainingDistance;
        var endX = sensorX + remainingDistance;
        if (beginX < 0)
        {
            beginX = 0;
        }
        if (endX > size)
        {
            endX = size - 1;
        }
        if (!rows[i].TryAdd(beginX, 1))
        {
            rows[i][beginX] += 1;
        }
        if (!rows[i].TryAdd(endX + 1, -1))
        {
            rows[i][endX + 1] -= 1;
        }
    }

    //DrawMap(dictionaryPart2);
}

var sum = 0;
var coverageNumber = 0;
for (int i = -5000000; i < 5000000; i++)
{
    if (dictionary.ContainsKey(i))
    {
        coverageNumber += dictionary[i];
    }

    if (coverageNumber > 0)
    {
        sum++;
    }
}

// part 1
var beaconAtTargetYCount = beaconPositions.Count(x => x.Item2 == targetY);
Console.WriteLine(sum - beaconAtTargetYCount);

var results = new List<(int, int)>();
for (int i = 0; i < size; i++)
{
    var list = rows[i].OrderBy(x => x.Key).ToList();
    coverageNumber = 0;

    foreach (var pair in list)
    {
        coverageNumber += pair.Value;
        if (coverageNumber < 1)
        {
            if (pair.Key < size)
            {
                Console.WriteLine($"Found result at {pair.Key} {i}");
                results.Add((pair.Key, i));
            }
        }
    }
    
    if (i % 100 == 0)
    {
        Console.WriteLine($"Line {i}");
    }
}

var result = results.Single();
Console.WriteLine((long)result.Item1 * 4_000_000 + result.Item2);

void DrawMap(Dictionary<(int, int), int>? dic)
{

    for (int y = 0; y < size; y++)
    {
        var coverageNumber = 0;
        for (int x = 0; x < size; x++)
        {
            if (dic.ContainsKey((x,y)))
            {
                coverageNumber += dic[(x, y)];
            }

            Console.Write(coverageNumber);
        }

        Console.WriteLine();
    }
    Console.WriteLine();
}