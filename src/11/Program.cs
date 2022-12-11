using System.Numerics;

var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var monkeys = new List<Monkey>();
var currentMonkey = new Monkey();
foreach (var line in lines)
{
    if (line == "")
    {
        monkeys.Add(currentMonkey);
        currentMonkey = new Monkey();
        continue;
    }

    if (line.StartsWith("Monkey "))
    {
        currentMonkey.Id = BigInteger.Parse(line.Split(" ")[1][..1]);
    }

    if (line.StartsWith("  Starting items:"))
    {
        var itemsString = line.Split("  Starting items: ")[1];
        currentMonkey.Items = itemsString.Split(", ").Select(x => new Item(BigInteger.Parse(x))).ToList();
    }

    if (line.StartsWith("  Operation:"))
    {
        var expression = line.Split("new = old ")[1];
        var operation = expression.Split(" ")[0];
        var operand = expression.Split(" ")[1];

        BigInteger OperationFunc(BigInteger a, BigInteger b) => operation == "+" ? a + b : a * b;

        currentMonkey.Operation = x =>
        {
            foreach (var dictionaryKey in x.Dictionary.Keys)
            {
                if (operand != "old")
                {
                    x.Dictionary[dictionaryKey] = OperationFunc(x.Dictionary[dictionaryKey], BigInteger.Parse(operand));
                }
                else
                {
                    x.Dictionary[dictionaryKey] = OperationFunc(x.Dictionary[dictionaryKey], x.Dictionary[dictionaryKey]);
                }
            }

            return x;
        };
    }

    if (line.StartsWith("  Test: "))
    {
        var divisibleBy = int.Parse(line.Split("by ")[1]);
        currentMonkey.Test = x =>
        {
            var isDivisible = x.Dictionary[divisibleBy] % divisibleBy == 0;

            foreach (var dictionaryKey in x.Dictionary.Keys)
            {
                if (x.Dictionary[dictionaryKey] % dictionaryKey == 0)
                {
                    x.Dictionary[dictionaryKey] = dictionaryKey;
                }
            }

            return isDivisible;
        };
    }

    if (line.StartsWith("    If true: "))
    {
        currentMonkey.TargetWhenTrue = BigInteger.Parse(line.Split("monkey ")[1]);
    }
    if (line.StartsWith("    If false: "))
    {
        currentMonkey.TargetWhenFalse = BigInteger.Parse(line.Split("monkey ")[1]);
    }
}
monkeys.Add(currentMonkey);

//for (int i = 0; i < 20; i++)
for (int i = 0; i < 10000; i++)
{
    foreach (var monkey in monkeys)
    {
        foreach (var monkeyItem in monkey.Items)
        {
            var newValue = monkey.Operation(monkeyItem);
            monkey.InspectionCounter++;
            
            var target = monkey.Test(newValue) ? monkey.TargetWhenTrue : monkey.TargetWhenFalse;
            monkeys.Single(x => x.Id == target).Items.Add(newValue);
        }

        monkey.Items = new List<Item>();
    }

    if (i % 10 == 0)
    {
        Console.WriteLine($"Round {i}");
    }
}

var mostActiveMonkeys = monkeys.OrderByDescending(x => x.InspectionCounter).Take(2).ToList();
var monkeyBusiness = mostActiveMonkeys.First().InspectionCounter * mostActiveMonkeys.Last().InspectionCounter;
Console.WriteLine(monkeyBusiness);

class Monkey
{
    public BigInteger Id { get; set; }
    public List<Item> Items { get; set; } = new();
    public Func<Item, Item> Operation { get; set; }
    public Func<Item, bool> Test { get; set; }
    public BigInteger TargetWhenTrue { get; set; }
    public BigInteger TargetWhenFalse { get; set; }
    public BigInteger InspectionCounter { get; set; } = 0;
}

class Item
{
    public BigInteger Initial { get; }
    public Dictionary<int, BigInteger> Dictionary { get; }
    public BigInteger Current { get; set; }

    public Item(BigInteger initial)
    {
        Initial = initial;
        Dictionary = new Dictionary<int, BigInteger>
        {
            [13] = Initial,
            [19] = Initial,
            [5] = Initial,
            [2] = Initial,
            [17] = Initial,
            [11] = Initial,
            [7] = Initial,
            [3] = Initial,

            //[23] = Initial
        };
        Current = initial;
    }
}