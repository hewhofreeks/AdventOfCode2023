using var inputStream = new StreamReader("input.txt");


while (!inputStream.EndOfStream)
{
    var source = inputStream.ReadLine().Split(':')[1].Trim().Split(' ').Select(long.Parse);

    List<(long seedStart, long seedEnd)> allSeeds = new List<(long seedStart, long seedEnd)>();
    for (int i = 0; i < source.Count(); i += 2)
    {
        allSeeds.Add((source.ElementAt(i), source.ElementAt(i) + source.ElementAt(i + 1)));
    }

    var minLocation = long.MaxValue;

    inputStream.ReadLine();
    inputStream.ReadLine();

    Stack<List<long[]>> mapStack = new Stack<List<long[]>>();
    List<long[]> maps = new List<long[]>();
    long largestLocation = long.MinValue;

    while (!inputStream.EndOfStream)
    {
        var line = inputStream.ReadLine();
        if (String.IsNullOrEmpty(line))
        {
            mapStack.Push(maps);
            maps = new List<long[]>();

            if (!inputStream.EndOfStream)
                inputStream.ReadLine(); // Read text
        }
        else
        {
            var toaddmaps = line.Split(' ').Select(long.Parse).ToArray();
            largestLocation = long.Max(largestLocation, long.Max(toaddmaps[0], toaddmaps[1]) + toaddmaps[2]);
            maps.Add(toaddmaps);
        }

        if (inputStream.EndOfStream)
        {
            mapStack.Push(maps);
        }
    }

    List<List<long[]>> allMaps = new List<List<long[]>>();

    while (mapStack.TryPop(out var map))
        allMaps.Add(map);

    // Order by Lowest
    //allMaps = allMaps.OrderBy(m => m[0]).ToList();

    List<List<MapNode>> allMapNodes = new List<List<MapNode>>();
    int index = 0;

    // Start with location, add in missing values
    foreach (var map in allMaps)
    {
        List<MapNode> mapNodes = new List<MapNode>();
        long start = 0;

        // Order by destination(source)
        var sorted = map.OrderBy(m => m[0]);

        foreach (var mapInOrder in sorted)
        {
            if (mapInOrder[0] != start)
            {
                // if node doesn't exist, add it
                mapNodes.Add(new MapNode { StartDestination = start, EndDestination = mapInOrder[0] - 1, StartSource = start, EndSource = mapInOrder[0] - 1, Index = index });
            }

            mapNodes.Add(new MapNode { StartDestination = mapInOrder[0], EndDestination = mapInOrder[0] + mapInOrder[2] - 1, StartSource = mapInOrder[1], EndSource = mapInOrder[1] + mapInOrder[2] - 1, Index = index });

            start = mapInOrder[0] + mapInOrder[2];
        }

        if (start < largestLocation)
        {
            mapNodes.Add(new MapNode { StartSource = start, StartDestination = start, EndDestination = largestLocation, EndSource = largestLocation, Index = index });
        }

        // 
        index++;
        allMapNodes.Add(mapNodes);
    }

    // Search Nodes
    Stack<MapNode> searchNodes = new Stack<MapNode>();

    // Go through locations and search
    var startNode = new MapNode() { StartSource = 0, StartDestination = 0, EndDestination = largestLocation, EndSource = largestLocation, Index = -1 };

    do
    {
        var diff = (startNode.EndSource - startNode.StartSource) / 2;

        var leftNode = new MapNode { StartSource = startNode.StartSource, EndSource = startNode.StartSource + diff, Index = startNode.Index };
        var rightNode = new MapNode { StartSource = startNode.StartSource + diff + 1, EndSource = startNode.EndSource, Index = startNode.Index };

        // Test Left

        var nodes = GetNodesInNextRow(leftNode, allMapNodes[0]);

        searchNodes = new Stack<MapNode>(nodes);
        bool found = false;
        while (searchNodes.TryPop(out var searchNode) && !found)
        {
            var nextNodes = GetNodesInNextRow(searchNode, allMapNodes[searchNode.Index]);

            if (nextNodes.Any(n => n.Index == 7))
            {
                // At the end, check against seeds
                foreach (var seed in allSeeds)
                {
                    if (nextNodes.Any(n => (n.StartSource <= seed.seedEnd && n.EndSource >= seed.seedEnd)
                    || (n.StartSource <= seed.seedStart && n.EndSource >= seed.seedStart)
                    || (n.StartSource >= seed.seedStart && n.EndSource <= seed.seedEnd)))
                    {
                        found = true;
                        break;
                    }
                }
            }
            else
                foreach (var n in nextNodes)
                    searchNodes.Push(n);
        }

        if(found)
        {
            startNode = leftNode;
            continue;
        }

        // No need to test right
        startNode = rightNode;

        // Loop until we find the final number
    } while (startNode.StartSource != startNode.EndSource);

    Console.WriteLine("Result: " + startNode.StartSource);

    // Read each mapping to the end end

    // Part 2
    //List<long> seeds = new List<long>();
    //for (int i = 0; i < source.Count(); i += 2)
    //{
    //    var initSeed = source.ElementAt(i);
    //    for (var j = 0; j < source.ElementAt(i + 1); j++)
    //    {
    //        var mapLine = 0;
    //        var currentSeed = initSeed + j;

    //        Dictionary<long, long> sourceToDestination = new Dictionary<long, long>();
    //        mapLine++;
    //        string input = "";
    //        var currentMapping = currentSeed;
    //        do
    //        {
    //            mapLine++;

    //            List<long[]> map = new List<long[]>();
    //            do
    //            {
    //                input = maps[mapLine++];

    //                if (!String.IsNullOrEmpty(input))
    //                    map.Add(input.Split(' ').Select(long.Parse).ToArray());

    //            } while (!String.IsNullOrEmpty(input) && mapLine < maps.Count());


    //            long destination = currentMapping;

    //            foreach (var mapping in map)
    //            {
    //                var minSource = mapping[1];
    //                var maxSource = mapping[1] + mapping[2] - 1;

    //                var minDest = mapping[0];

    //                if (currentMapping >= minSource && currentMapping <= maxSource)
    //                {
    //                    var diff = currentMapping - minSource;

    //                    destination = minDest + diff;
    //                    break;
    //                }
    //            }

    //            currentMapping = destination;


    //        } while (mapLine < maps.Count());

    //        minLocation = long.Min(currentMapping, minLocation);
    //    }
    //}
    Console.WriteLine("Result: " + minLocation);
}

List<MapNode> GetNodesInNextRow(MapNode startNode, List<MapNode> nextRow)
{
    List<MapNode> retNodes = new List<MapNode>();

    foreach (var nodeToCheck in nextRow)
    {
        // Contains all values in startNode
        if (startNode.StartSource >= nodeToCheck.StartDestination && startNode.EndSource <= nodeToCheck.EndDestination)
        {
            var startDiff = startNode.StartSource - nodeToCheck.StartDestination;
            var endDiff = nodeToCheck.EndDestination - startNode.EndSource;

            retNodes.Add(new MapNode { StartSource = nodeToCheck.StartSource + startDiff, EndSource = nodeToCheck.EndSource - endDiff, Index = nodeToCheck.Index + 1 });
        }

        // Contains all values in nodeToCheck
        else if (startNode.StartSource <= nodeToCheck.StartDestination && startNode.EndSource >= nodeToCheck.EndDestination)
        {
            retNodes.Add(new MapNode { StartSource = nodeToCheck.StartSource, EndSource = nodeToCheck.EndSource, Index = nodeToCheck.Index + 1 });
        }

        // Contains first values
        else if (startNode.StartSource <= nodeToCheck.EndDestination && startNode.EndSource > nodeToCheck.EndDestination)
        {
            var diff = nodeToCheck.EndDestination - startNode.StartSource;
            retNodes.Add(new MapNode { StartSource = nodeToCheck.StartSource, EndSource = nodeToCheck.StartSource + diff, Index = nodeToCheck.Index + 1 });
        }

        // Contains last values
        else if (startNode.EndSource >= nodeToCheck.StartDestination && startNode.StartSource < nodeToCheck.StartDestination)
        {
            var endDiff = startNode.EndSource - nodeToCheck.StartDestination;
            retNodes.Add(new MapNode { StartSource = nodeToCheck.StartSource, EndSource = nodeToCheck.StartSource + endDiff, Index = nodeToCheck.Index + 1 });
        }
    }

    return retNodes;
}

//Dictionary<long, long> sourceToDestination = new Dictionary<long, long>();
//inputStream.ReadLine();
//string input = "";

//do
//{
//    inputStream.ReadLine();

//    List<long[]> map = new List<long[]>();
//    do
//    {
//        input = inputStream.ReadLine();
//        if (!String.IsNullOrEmpty(input))
//            map.Add(input.Split(' ').Select(long.Parse).ToArray());

//    } while (!String.IsNullOrEmpty(input));

//    foreach (var sourceItem in source)
//    {
//        long destination = 0;

//        foreach (var mapping in map)
//        {
//            var minSource = mapping[1];
//            var maxSource = mapping[1] + mapping[2] - 1;

//            var minDest = mapping[0];

//            if (sourceItem >= minSource && sourceItem <= maxSource)
//            {
//                var diff = sourceItem - minSource;

//                destination = minDest + diff;
//            }
//        }

//        sourceToDestination.Add(sourceItem, destination != 0 ? destination : sourceItem);
//    }

//    source = sourceToDestination.Values.ToArray();
//    sourceToDestination.Clear();

//} while (!inputStream.EndOfStream) ;

//Console.WriteLine("Result: " + source.Order().First());
//}

class MapNode
{
    public long StartDestination { get; set; }

    public long EndDestination { get; set; }

    public long StartSource { get; set; }

    public long EndSource { get; set; }

    public int Index { get; set; }
}