using System.Diagnostics;

var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);



var sum = 0;
for (int i = 0; i < lines.Length / 3 + 1; i++)
{
    var first = lines[i * 3];
    var second = lines[i * 3 + 1];

    Logger.Log($"== Pair {i + 1} ==");

    var result = new MyComparer().Compare(first, second);

    if (result == 0)
    {
        throw new Exception("Same inputs");
    }

    if (result < 0)
    {
        sum += i + 1;
    }
}

//part1
Console.WriteLine(sum);

var linesList = lines.ToList();
var packet2 = "[[2]]";
var packet6 = "[[6]]";
linesList.Add(packet2);
linesList.Add(packet6);
var list = linesList.Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x, new MyComparer()).ToList();
var pos2 = list.IndexOf(packet2) +1;
var pos6 = list.IndexOf(packet6) +1;

Console.WriteLine(pos2*pos6);

void Log(string s)
{
    Logger.Log(s);
}

class MyComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        var b = CompareInternal(x, y);
        if (b == null)
        {
            return 0;
        }

        return b.Value ? -1 : 1;
    }

    bool? CompareInternal(string first, string second)
    {
        bool IsArray(string s) => s.StartsWith("[");

        Logger.Log($"- Compare {first} vs {second}");
        if (IsArray(first) && IsArray(second))
        {
            var firstA = Split(first);
            var secondA = Split(second);

            for (int i = 0; i < firstA.Length; i++)
            {
                if (i == secondA.Length)
                {
                    Logger.Log("- Right side ran out of items, so inputs are not in the right order");
                    return false;
                }

                var compare = CompareInternal(firstA[i], secondA[i]);
                if (compare != null)
                {
                    return compare;
                }
            }

            if (firstA.Length == secondA.Length)
            {
                return null;
            }

            Logger.Log("Left side ran out of items, so inputs are in the right order");
            return true;
        }

        var firstParsed = int.TryParse(first, out var firstNum);
        var secondParsed = int.TryParse(second, out var secondNum);
        if (firstParsed && secondParsed)
        {
            if (firstNum == secondNum)
            {
                return null;
            }
            else
            {
                var compare = firstNum < secondNum;
                if (compare)
                {
                    Logger.Log("- Left side is smaller, so inputs are in the right order");
                    return true;
                }
                else
                {
                    Logger.Log("- Right side is smaller, so inputs are not in the right order");
                    return false;
                }
            }
        }

        if (!firstParsed)
        {
            Debug.Assert(IsArray(first));
            Debug.Assert(secondParsed);

            second = Box(second);
            Logger.Log($"- Mixed types; convert right to {second} and retry comparison");
            return CompareInternal(first, second);
        }
        else
        {
            Debug.Assert(IsArray(second));
            Debug.Assert(firstParsed);

            first = Box(first);
            Logger.Log($"- Mixed types; convert left to {first} and retry comparison");
            return CompareInternal(first, second);
        }
    }

    string[] Split(string s)
    {
        s = Unbox(s);

        if (s.Length == 0)
        {
            return Array.Empty<string>();
        }

        var results = new List<string>();
        var depth = 0;
        var lastElementEnd = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '[')
            {
                depth++;
            }

            if (s[i] == ']')
            {
                depth--;
            }

            if (depth == 0 && s[i] == ',')
            {
                results.Add(s[lastElementEnd..i]);
                lastElementEnd = i + 1;
            }
        }

        results.Add(s[lastElementEnd..]);

        return results.ToArray();
    }

    string Box(string s)
    {
        return $"[{s}]";
    }

    string Unbox(string s)
    {
        //var open = s.IndexOf("[");
        //var close = s.IndexOf("]");
        if (s[0] == '[' && s[^1] == ']')
        {
            return s[1..^1];
        }
        else
        {
            return s;
        }
    }
}

static class Logger
{
    public static void Log(string s)
    {
        //Console.WriteLine(s);
    }
}