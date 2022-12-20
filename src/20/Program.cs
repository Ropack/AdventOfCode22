var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

// Part 1
var list = lines.Select((x, i) => new N(i, int.Parse(x))).ToList();
var sequence = list.ToList();
Mix(list, sequence);

var sum = GetSum(sequence);
Console.WriteLine(sum);


// Part 2
var key = 811589153;
var listPart2 = list.Select(x => x with { Value = x.Value * key }).ToList();
var sequencePart2 = listPart2.ToList();
for (int i = 0; i < 10; i++)
{
    Mix(listPart2, sequencePart2);
}

var sumPart2 = GetSum(sequencePart2);
Console.WriteLine(sumPart2);

void Mix(List<N> ns, List<N> sequence1)
{
    foreach (var n in ns)
    {
        var index = sequence1.IndexOf(n);
        sequence1.RemoveAt(index);
        var targetIndex = index + n.Value;
        if (targetIndex < 0)
        {
            targetIndex = sequence1.Count + targetIndex % sequence1.Count;
        }

        if (targetIndex > sequence1.Count - 1)
        {
            targetIndex %= sequence1.Count;
        }

        sequence1.Insert((int)targetIndex, n);
    }
}

long GetSum(List<N> list1)
{
    var zeroElement = list1.Single(x => x.Value == 0);
    var indexOfZero = list1.IndexOf(zeroElement);
    var pos1 = (indexOfZero + 1000) % list1.Count;
    var pos2 = (indexOfZero + 2000) % list1.Count;
    var pos3 = (indexOfZero + 3000) % list1.Count;
    var l = list1[pos1].Value + list1[pos2].Value + list1[pos3].Value;
    return l;
}

record N(int Id, long Value);
