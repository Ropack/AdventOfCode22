var input = "input.txt";
var lines = File.ReadAllLines(input);

var line = lines[0];
//var line = "bvwbjplbgvbhsrlpgdmjqwftvncz"; // 5
//var line = "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"; // 11
var part1 = GetIndexOfNDistinctCharacters(line, 4);
Console.WriteLine(part1);

var part2 = GetIndexOfNDistinctCharacters(line, 14);
Console.WriteLine(part2);

int GetIndexOfNDistinctCharacters(string s, int n)
{
    for (int i = n; i < s.Length; i++)
    {
        if (s.Substring(i - n, n).ToCharArray().Distinct().Count() == n)
        {
            return i;
        }
    }
    return -1;
}