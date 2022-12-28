var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var baseChars = new[] { '=', '-', '0', '1', '2' };
checked
{
	var sum = 0L;
	foreach (var line in lines)
	{
		var dec = StringToInt(line);
		sum += dec;
	}

	var result = IntToString(sum);
	Console.WriteLine(result);

	long StringToInt(string s)
	{
		var result = 0L;
		for (int i = s.Length - 1, placeIndex = 0; i >= 0; i--, placeIndex++)
		{
			var c = s[i];
			var charIndex = Array.IndexOf(baseChars, c);
			var charValue = charIndex - 2;
			var placePower = (long)Math.Pow(baseChars.Length, placeIndex);
			result += charValue * placePower;
		}

		return result;
	}

	string IntToString(long value)
	{
		string result = string.Empty;
		int targetBase = baseChars.Length;

		do
		{
			value += 2;
			result = baseChars[value % targetBase] + result;
			value /= targetBase;
		} while (value > 0);

		return result;
	}
}