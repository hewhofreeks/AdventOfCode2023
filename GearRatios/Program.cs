using var inputStream = new StreamReader("input.txt");

char[][] input = new char[140][];
int lineNum = 0;

// Read input
while (!inputStream.EndOfStream)
{
    var line = inputStream.ReadLine();
    input[lineNum] = line.ToArray();

    lineNum++;
}

List<int> partNums = new List<int>();
Dictionary<string, List<int>> gears = new Dictionary<string, List<int>>();

// Go line by line to find each number
for (int i = 0; i < input.Length; i++)
{
    string currentNumber = "";
    int currentNumberStartIndex = -1;
    int currentNumberEndIndex = -1;

    for (int j = 0; j < input[i].Length + 1; j++)
    {
        // is digit and not the end
        if (j != 140 && input[i][j] >= '0' && input[i][j] <= '9')
        {
            // If first character in number
            if (String.IsNullOrEmpty(currentNumber))
                currentNumberStartIndex = j;

            currentNumber += input[i][j];
        }
        else if (!String.IsNullOrEmpty(currentNumber))
        {
            // If end of number
            currentNumberEndIndex = j - 1;
            bool foundSymbol = false;

            // check above
            if (i > 0)
            {
                for (int checkJ = int.Max(0, currentNumberStartIndex - 1); checkJ <= int.Min(139, currentNumberEndIndex + 1); checkJ++)
                {
                    var checkValue = input[i - 1][checkJ];
                    if (IsSymbol(checkValue))
                    {
                        foundSymbol = true;
                        if (checkValue == '*')
                        {
                            AddNumberToGear(i - 1, checkJ, int.Parse(currentNumber));
                        }
                    }
                }
            }

            // check below
            if (i < 139)
            {
                for (int checkJ = int.Max(0, currentNumberStartIndex - 1); checkJ <= int.Min(139, currentNumberEndIndex + 1); checkJ++)
                {
                    var checkValue = input[i + 1][checkJ];
                    if (IsSymbol(checkValue))
                    {
                        foundSymbol = true;

                        if (checkValue == '*')
                        {
                            AddNumberToGear(i + 1, checkJ, int.Parse(currentNumber));
                        }
                    }
                }
            }

            // check left
            if (currentNumberStartIndex > 0)
            {
                if (IsSymbol(input[i][currentNumberStartIndex - 1]))
                {
                    foundSymbol = true;

                    if (input[i][currentNumberStartIndex - 1] == '*')
                    {
                        AddNumberToGear(i, currentNumberStartIndex - 1, int.Parse(currentNumber));
                    }
                }
            }

            // check right
            if (currentNumberEndIndex < 139)
            {
                if (IsSymbol(input[i][currentNumberEndIndex + 1]))
                {
                    foundSymbol = true;

                    if (input[i][currentNumberEndIndex + 1] == '*')
                    {
                        AddNumberToGear(i, currentNumberEndIndex + 1, int.Parse(currentNumber));
                    }
                }
            }

            if (foundSymbol)
            {
                partNums.Add(int.Parse(currentNumber));
            }

            currentNumber = "";
            currentNumberStartIndex = -1;
            currentNumberEndIndex = -1;
        }
    }
}


Console.WriteLine("Result 1: " + partNums.Sum());

Console.WriteLine("Result 2: " + gears.Where(g => g.Value.Count() == 2).Select(g => g.Value[0] * g.Value[1]).Sum());


void AddNumberToGear(int gearY, int gearX, int val)
{
    if (!gears.ContainsKey($"{gearY},{gearX}"))
        gears.Add($"{gearY},{gearX}", new List<int>());

    gears[$"{gearY},{gearX}"].Add(val);
}

bool IsSymbol(char val)
{
    return !(val >= '0' && val <= '9') && val != '.';
}