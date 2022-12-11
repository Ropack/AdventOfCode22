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
        currentMonkey.Id = Convert.ToInt32(line.Split(" ")[1][..1]);
    }

    if (line.StartsWith("  Starting items:"))
    {
        var itemsString = line.Split("  Starting items: ")[1];
        currentMonkey.Items = itemsString.Split(", ").Select(int.Parse).ToList();
    }

    if (line.StartsWith("  Operation:"))
    {
        var expression = line.Split("new = old ")[1];
        var operation = expression.Split(" ")[0];
        var operand = expression.Split(" ")[1];

        int OperationFunc(int a, int b) => operation == "+" ? a + b : a * b;

        currentMonkey.Operation = x => OperationFunc(x, operand == "old" ? x : int.Parse(operand));
    }

    if (line.StartsWith("  Test: "))
    {
        var divisibleBy = int.Parse(line.Split("by ")[1]);
        currentMonkey.Test = x => x % divisibleBy == 0;
    }
    
    if (line.StartsWith("    If true: "))
    {
        currentMonkey.TargetWhenTrue = int.Parse(line.Split("monkey ")[1]);
    }
    if (line.StartsWith("    If false: "))
    {
        currentMonkey.TargetWhenFalse = int.Parse(line.Split("monkey ")[1]);
    }
}
monkeys.Add(currentMonkey);

for (int i = 0; i < 20; i++)
{
    foreach (var monkey in monkeys)
    {
        foreach (var monkeyItem in monkey.Items)
        {
            var newValue = monkey.Operation(monkeyItem);
            monkey.InspectionCounter++;
            newValue /= 3;
            var target = monkey.Test(newValue) ? monkey.TargetWhenTrue : monkey.TargetWhenFalse;
            monkeys.Single(x => x.Id == target).Items.Add(newValue);
        }

        monkey.Items = new List<int>();
    }
}

var mostActiveMonkeys = monkeys.OrderByDescending(x => x.InspectionCounter).Take(2).ToList();
var monkeyBusiness = mostActiveMonkeys.First().InspectionCounter * mostActiveMonkeys.Last().InspectionCounter;
Console.WriteLine(monkeyBusiness);

class Monkey
{
    public int Id { get; set; }
    public List<int> Items { get; set; } = new();
    public Func<int, int> Operation { get; set; }
    public Func<int, bool> Test { get; set; }
    public int TargetWhenTrue { get; set; }
    public int TargetWhenFalse { get; set; }
    public int InspectionCounter { get; set; } = 0;
}