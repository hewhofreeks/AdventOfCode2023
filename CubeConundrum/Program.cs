int maxRed = 12;
int maxGreen = 13;
int maxBlue = 14;

using var inputStream = new StreamReader("input.txt");

List<int> gamesPossible = new List<int>();
List<int> allPowers = new List<int>();
int gameNum = 1;

while (!inputStream.EndOfStream)
{ 
    var line = inputStream.ReadLine();


    // Is Game allowed?
    bool gameAllowed = true;
    var rounds = line.Split(':')[1];
    var games = rounds.Split(';');
    int maxRedInGame = int.MinValue;
    int maxBlueInGame = int.MinValue;
    int maxGreenInGame = int.MinValue;

    foreach (var game in games)
    {
        var pulls = game.Trim().Split(",");
        foreach(var pull in pulls)
        {
            var result = pull.Trim().Split(" ");

            int maxAllowed = 0;
            switch(result[1])
            {
                case "red":
                    maxAllowed = maxRed;
                    maxRedInGame = int.Max(maxRedInGame, int.Parse(result[0]));
                    break;
                case "blue":
                    maxAllowed = maxBlue;
                    maxBlueInGame = int.Max(maxBlueInGame, int.Parse(result[0]));
                    break;
                case "green":
                    maxAllowed = maxGreen;
                    maxGreenInGame = int.Max(maxGreenInGame, int.Parse(result[0]));
                    break;
            }

            if (int.Parse(result[0]) > maxAllowed)
                gameAllowed = false;
        }

        
    }

    var power = maxRedInGame * maxBlueInGame * maxGreenInGame;
    allPowers.Add(power);

    if (gameAllowed)
        gamesPossible.Add(gameNum);

    gameNum++;
}

Console.WriteLine("Result 1: " + gamesPossible.Sum());
Console.WriteLine("Results 2: " + allPowers.Sum());