var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var list = lines.Select((x, i) => new N(i, int.Parse(x))).ToList();

var sequence = list.ToList();

foreach (var n in list)
{
    var index = sequence.IndexOf(n);
    sequence.RemoveAt(index);
    var targetIndex = index + n.Value;
    if (targetIndex < 0)
    {
        targetIndex = list.Count + targetIndex % list.Count - 1;
    }

    if (targetIndex > sequence.Count - 1)
    {
        targetIndex = targetIndex % list.Count + 1;
    }
    sequence.Insert(targetIndex, n);
}

var zeroElement = sequence.Single(x => x.Value == 0);
var indexOfZero = sequence.IndexOf(zeroElement);
var pos1 = (indexOfZero + 1000) % sequence.Count;
var pos2 = (indexOfZero + 2000) % sequence.Count;
var pos3 = (indexOfZero + 3000) % sequence.Count;
var sum = sequence[pos1].Value + sequence[pos2].Value + sequence[pos3].Value;
Console.WriteLine(sum);

record N(int Id, int Value);