
using System.Data;

using var inputStream = new StreamReader("input.txt");

int total = 0;

Dictionary<string, string> Nums = new Dictionary<string, string>()
{
    { "one", "1" },
    { "two", "2" },
    { "three", "3" },
    { "four", "4" },
    { "five", "5" },
    { "six", "6" },
    { "seven", "7" },
    { "eight", "8" },
    { "nine", "9" }
};

while (!inputStream.EndOfStream)
{
    var line = inputStream.ReadLine();

    string first, last;

    // Probably could have done with two pointers doing the same thing but i got lazy
    first = FindFirstNumber(line);
    last = FindLastNumber(line);

    total += int.Parse(first.ToString() + last.ToString());
}

Console.WriteLine("Result: " + total);

string PopulateNumbersInLine(string line)
{
    var newLine = line;

    foreach(var num in Nums)
    {
        newLine = newLine.Replace(num.Key, num.Value);
    }

    return newLine;
}

string FindFirstNumber(string line)
{
    string temp = "";
    int num = 0;
    for(int i = 0;i<line.Length;i++)
    {
        // If current digit is num
        if (int.TryParse(line[i].ToString(), out num))
            break;

        temp += line[i];

        // Part 2
        foreach(var wordNum in Nums)
        {
            if(temp.Contains(wordNum.Key))
                return wordNum.Value;
        }
    }

    return num.ToString();
}

string FindLastNumber(string line)
{
    string temp = "";
    int num = 0;
    for (int i = line.Length - 1; i >= 0; i--)
    {
        // If current digit is num
        if (int.TryParse(line[i].ToString(), out num))
            break;

        temp = line[i] + temp;

        // Part 2
        foreach (var wordNum in Nums)
        {
            if (temp.Contains(wordNum.Key))
                return wordNum.Value;
        }
    }

    return num.ToString();
}
