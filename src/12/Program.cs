
using System.ComponentModel;

var input = "input.txt";
//var input = "test-input.txt";
var lines = File.ReadAllLines(input);

var sizeX = lines[0].Length;
var sizeY = lines.Length;
var map = new int[sizeX, sizeY];
var startX = 0;
var startY = 0;

for (int i = 0; i < sizeY; i++)
{
    var line = lines[i];
    for (int j = 0; j < sizeX; j++)
    {
        if (line[j] == 'S')
        {
            map[j, i] = 0;
            startX = j;
            startY = i;
        }
        else if (line[j] == 'E')
        {
            map[j, i] = 27;
        }
        else
        {
            map[j, i] = line[j] - 96;
        }
    }
}

var initialArray = new bool[sizeX, sizeY];
initialArray[startX, startY] = true;
//var result = F(startX, startY, initialArray, int.MaxValue);

var q = new Queue<(int x, int y, bool[,] visited, int currentLength)>();
q.Enqueue((startX, startY, initialArray, 0));
while (q.TryDequeue(out var item))
{
    var x = item.x;
    var y = item.y;
    var current = map[x, y];
    var visited = item.visited;
    //right
    {
        var newX = x + 1;
        var newY = y;
        if (x < sizeX - 1 && current > map[newX, newY] - 2 && !visited[newX, newY])
        {
            visited[newX, newY] = true;
            var sum = item.currentLength + 1;
            if (map[newX, newY] == 27)
            {
                Console.WriteLine($"Found end, returning {sum}");
                return;
            }

            q.Enqueue((newX, newY, visited, sum));
        }
    }

    // left
    {
        var newX = x - 1;
        var newY = y;
        if (x > 0 && current > map[newX, newY] - 2 && !visited[newX, newY])
        {
            visited[newX, newY] = true;
            var sum = item.currentLength + 1;
            if (map[newX, newY] == 27)
            {
                Console.WriteLine($"Found end, returning {sum}");
                return;
            }

            q.Enqueue((newX, newY, visited, sum));
        }
    }

    // bottom
    {
        var newX = x;
        var newY = y + 1;
        if (y < sizeY - 1 && current > map[newX, newY] - 2 && !visited[newX, newY])
        {
            visited[newX, newY] = true;
            var sum = item.currentLength + 1;
            if (map[newX, newY] == 27)
            {
                Console.WriteLine($"Found end, returning {sum}");
                return;
            }

            q.Enqueue((newX, newY, visited, sum));
        }
    }

    // up
    {
        var newX = x;
        var newY = y - 1;
        if (y > 0 && current > map[newX, newY] - 2 && !visited[newX, newY])
        {
            visited[newX, newY] = true;
            var sum = item.currentLength + 1;
            if (map[newX, newY] == 27)
            {
                Console.WriteLine($"Found end, returning {sum}");
                return;
            }

            q.Enqueue((newX, newY, visited, sum));
        }
    }
}

//Console.WriteLine(result - 1);

//int F(int x, int y, bool[,] visited, int currentMin)
//{
//    var current = map[x, y];
//    Console.WriteLine($"{x} {y} {current}");
//    var results = new List<int>()
//    {
//        currentMin
//    };

//    var sumThreshold = 300;

//    //right
//    {
//        var newX = x + 1;
//        var newY = y;
//        if (x < sizeX - 1 && current > map[newX, newY] - 2 && !visited[newX, newY])
//        {
//            var array = new bool[sizeX, sizeY];
//            Buffer.BlockCopy(visited, 0, array, 0, visited.Length * sizeof(bool));
//            array[newX, newY] = true;
//            var sum = array.Cast<bool>().Sum(b => b ? 1 : 0);
//            if (map[newX, newY] == 27)
//            {
//                Console.WriteLine($"Found end, returning {sum}");
//                return sum;
//            }

//            if (sum < currentMin)
//            {
//                if (sum < sumThreshold)
//                {
//                    results.Add(F(newX, newY, array, results.Min()));
//                }
//                else
//                {
//                    Console.WriteLine($"Sum is over {sum}, skipping");
//                }
//            }
//        }
//    }

//    // left
//    {
//        var newX = x - 1;
//        var newY = y;
//        if (x > 0 && current > map[newX, newY] - 2 && !visited[newX, newY])
//        {
//            var array = new bool[sizeX, sizeY];
//            Buffer.BlockCopy(visited, 0, array, 0, visited.Length * sizeof(bool));
//            array[newX, newY] = true;
//            var sum = array.Cast<bool>().Sum(b => b ? 1 : 0);
//            if (map[newX, newY] == 27)
//            {
//                Console.WriteLine($"Found end, returning {sum}");
//                return sum;
//            }

//            if (sum < currentMin)
//            {
//                if (sum < sumThreshold)
//                {
//                    results.Add(F(newX, newY, array, results.Min()));
//                }
//                else
//                {
//                    Console.WriteLine($"Sum is over {sum}, skipping");
//                }
//            }
//        }
//    }

//    // bottom

//    {
//        var newX = x;
//        var newY = y + 1;
//        if (y < sizeY - 1 && current > map[newX, newY] - 2 && !visited[newX, newY])
//        {
//            var array = new bool[sizeX, sizeY];
//            Buffer.BlockCopy(visited, 0, array, 0, visited.Length * sizeof(bool));
//            array[newX, newY] = true;
//            var sum = array.Cast<bool>().Sum(b => b ? 1 : 0);
//            if (map[newX, newY] == 27)
//            {
//                Console.WriteLine($"Found end, returning {sum}");
//                return sum;
//            }

//            if (sum < currentMin)
//            {
//                if (sum < sumThreshold)
//                {
//                    results.Add(F(newX, newY, array, results.Min()));
//                }
//                else
//                {
//                    Console.WriteLine($"Sum is over {sum}, skipping");
//                }
//            }
//        }
//    }

//    // up
//    {
//        var newX = x;
//        var newY = y - 1;
//        if (y > 0 && current > map[newX, newY] - 2 && !visited[newX, newY])
//        {
//            var array = new bool[sizeX, sizeY];
//            Buffer.BlockCopy(visited, 0, array, 0, visited.Length * sizeof(bool));
//            array[newX, newY] = true;
//            var sum = array.Cast<bool>().Sum(b => b ? 1 : 0);
//            if (map[newX, newY] == 27)
//            {
//                Console.WriteLine($"Found end, returning {sum}");
//                return sum;
//            }

//            if (sum < currentMin)
//            {
//                if (sum < sumThreshold)
//                {
//                    results.Add(F(newX, newY, array, results.Min()));
//                }
//                else
//                {
//                    Console.WriteLine($"Sum is over {sum}, skipping");
//                }
//            }
//        }
//    }


//    return results.Min();
//}