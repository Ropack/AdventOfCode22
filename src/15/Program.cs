var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var targetY = 2000000;
//var targetY = 10;
var dictionary = new Dictionary<int, int>();
var beaconPositions = new HashSet<(int, int)>();
foreach (var line in lines)
{
    var sensorX = int.Parse(line.Split(' ')[2][2..^1]);
    var sensorY = int.Parse(line.Split(' ')[3][2..^1]);
    var beaconX = int.Parse(line.Split(' ')[8][2..^1]);
    var beaconY = int.Parse(line.Split(' ')[9][2..]);

    //if (beaconY == targetY)
    //{
    //    beaconAtTargetYCount++;
    //}
    beaconPositions.Add((beaconX, beaconY));

    var distance = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);

    var distanceToTargetY = Math.Abs(sensorY - targetY);
    if (distanceToTargetY > distance)
    {
        continue;
    }

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
