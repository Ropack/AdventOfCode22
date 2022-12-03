var lines = File.ReadAllLines("input.txt");

//part one
var score = 0;
foreach (var line in lines)
{
    var first = line.Split(" ")[0];
    var second = line.Split(" ")[1];

    // draw
    if (first == "A" && second == "X" ||
        first == "B" && second == "Y" ||
        first == "C" && second == "Z")
    {
        score += 3;
    }

    // win
    if (first == "A" && second == "Y" ||
        first == "B" && second == "Z" ||
        first == "C" && second == "X")
    {
        score += 6;
    }

    score += second[0] - 87;
}

Console.WriteLine(score);

// part two
var score2 = 0;
foreach (var line in lines)
{
    var first = line.Split(" ")[0];
    var second = line.Split(" ")[1];
    var my = "";

    // lose
    if (second == "X")
    {
        my = first switch
        {
            "A" => "Z",
            "B" => "X",
            "C" => "Y",
        };
    }

    // draw
    if (second == "Y")
    {
        my = first switch
        {
            "A" => "X",
            "B" => "Y",
            "C" => "Z",
        };

        score2 += 3;
    }

    // win
    if (second == "Z")
    {
        my = first switch
        {
            "A" => "Y",
            "B" => "Z",
            "C" => "X",
        };

        score2 += 6;
    }

    score2 += my[0] - 87;
}

Console.WriteLine(score2);