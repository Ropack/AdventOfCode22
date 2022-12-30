var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input).ToList();

checked
{
	var rootMonkey = GetMonkey("root");

	// part 1
	Console.WriteLine(rootMonkey.GetValue());

	// part2
	var monkeyOnPathToHuman = FindHuman(rootMonkey);
	long wantedResult = 0;
	if (rootMonkey.Monkey1.IsPathToHuman)
	{
		wantedResult = rootMonkey.Monkey2.GetValue();
	}
	else if (rootMonkey.Monkey2.IsPathToHuman)
	{
		wantedResult = rootMonkey.Monkey1.GetValue();
	}
	var humanNumber = GetHumanNumber(rootMonkey, wantedResult);
	Console.WriteLine(humanNumber);

	long GetHumanNumber(Monkey monkey, long wantedResult)
	{
		if (monkey.Id == "humn")
		{
			return wantedResult;
		}
		var path = monkey.Monkey1.IsPathToHuman ? monkey.Monkey1 : monkey.Monkey2;
		var nextResult = path.GetHumanPathOperand(wantedResult);
		return GetHumanNumber(path, nextResult);
	}

	Monkey? FindHuman(Monkey monkey)
	{
		if (monkey.Id == "humn")
		{
			monkey.IsPathToHuman = true;
			return monkey;
		}

		if (monkey.Operation == '0')
		{
			return null;
		}

		var l = FindHuman(monkey.Monkey1);
		var r = FindHuman(monkey.Monkey2);
		if (l != null)
		{
			monkey.IsPathToHuman = true;
			return l;
		}
		if (r != null)
		{
			monkey.IsPathToHuman = true;
			return r;
		}

		return null;
	}

	Monkey GetMonkey(string name)
	{
		var line = lines.Single(x => x.Split(": ")[0] == name);
		if (long.TryParse(line.Split(": ")[1], out var number))
		{
			return new Monkey()
			{
				Value = number,
				Operation = '0',
				Id = name
			};
		}
		var expression = line.Split(": ")[1].Split(" ");

		return new Monkey()
		{
			Monkey1 = GetMonkey(expression[0]),
			Monkey2 = GetMonkey(expression[2]),
			Operation = expression[1][0],
			Id = name
		};
	}
}
class Monkey
{
	public required string Id { get; set; }
	public Monkey? Monkey1 { get; set; }
	public Monkey? Monkey2 { get; set; }
	public char Operation { get; set; }
	public long Value { get; set; }
	public bool IsPathToHuman { get; set; }

	public long GetValue()
	{
		checked
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

	public long GetHumanPathOperand(long wantedResult)
	{
		checked
		{
			if (Monkey1?.IsPathToHuman ?? false)
			{
				return Operation switch
				{
					'0' => wantedResult,
					'+' => wantedResult - Monkey2.GetValue(),
					'-' => wantedResult + Monkey2.GetValue(),
					'*' => wantedResult / Monkey2.GetValue(),
					'/' => wantedResult * Monkey2.GetValue(),
					_ => throw new ArgumentOutOfRangeException()
				};
			}

			return Operation switch
			{
				'0' => wantedResult,
				'+' => wantedResult - Monkey1.GetValue(),
				'-' => Monkey1.GetValue() - wantedResult,
				'*' => wantedResult / Monkey1.GetValue(),
				'/' => Monkey1.GetValue() / wantedResult,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}