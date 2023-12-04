
using var inputStream = new StreamReader("input.txt");
int totalScore = 0;

Dictionary<int, int> totalScratchCards = new Dictionary<int, int>();

// Initial card count
for(int i=1;i<=204;i++)
{
    totalScratchCards.Add(i, 1);
}

int cardNum = 1;
while (!inputStream.EndOfStream)
{
    var line = inputStream.ReadLine();
    var cardParts = line.Split(':');
    var numbers = cardParts[1].Split('|');
    var winningNumbers = numbers[0].Split(' ').Where(s => !String.IsNullOrEmpty(s));
    var numbersOnCard = numbers[1].Split(' ').Where(s => !String.IsNullOrEmpty(s));

    HashSet<string> winners = new HashSet<string>(winningNumbers);

    int score = 0;

    // Part 1
    //foreach (var number in numbersOnCard)
    //{
    //    if(winners.Contains(number))
    //    {
    //        if (score == 0)
    //            score = 1;
    //        else
    //            score = score * 2;
    //    }
    //}

    // Part 2
    foreach (var number in numbersOnCard)
    {
        if (winners.Contains(number))
            score++;
    }
    

    for(int i= cardNum + 1; i <= cardNum + score; i++)
        totalScratchCards[i] += totalScratchCards[cardNum];

    //totalScore += score;
    cardNum++;
}

//Console.WriteLine("Result 1: " + totalScore);
Console.WriteLine("Result 2: " + totalScratchCards.Select(s => s.Value).Sum());