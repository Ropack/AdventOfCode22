var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input).ToList();

var rootMonkey = GetMonkey("root");

// part 1
Console.WriteLine(rootMonkey.GetValue());

Monkey GetMonkey(string name)
{
	var line = lines.Single(x => x.Split(": ")[0] == name);
	if (int.TryParse(line.Split(": ")[1], out var number))
	{
		return new Monkey()
		{
			Value = number,
			Operation = '0'
		};
	}
	var expression = line.Split(": ")[1].Split(" ");

	return new Monkey()
	{
		Monkey1 = GetMonkey(expression[0]),
		Monkey2 = GetMonkey(expression[2]),
		Operation = expression[1][0]
	};
}

class Monkey
{
	public Monkey? Monkey1 { get; set; }
	public Monkey? Monkey2 { get; set; }
	public char Operation { get; set; }
	public int Value { get; set; }
	public long GetValue()
	{
		return Operation switch
		{
			'0' => Value,
			'+' => Monkey1.GetValue() + Monkey2.GetValue(),
			'-' => Monkey1.GetValue() - Monkey2.GetValue(),
			'*' => Monkey1.GetValue() * Monkey2.GetValue(),
			'/' => Monkey1.GetValue() / Monkey2.GetValue(),
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}