namespace CleaningApi.Services;

public class CleaningService
{
    public int Clean(CleaningInstructions cleaningInstructions)
    {
        int uniqueCleanedSpots = 1;
        var xAxisCache = new Dictionary<int, HashSet<int>>();
        var yAxisCache = new Dictionary<int, HashSet<int>>();

        xAxisCache.Add(cleaningInstructions.Start.X, new HashSet<int> { cleaningInstructions.Start.Y });
        yAxisCache.Add(cleaningInstructions.Start.Y, new HashSet<int> { cleaningInstructions.Start.X });

        (int X, int Y) startPosition = (cleaningInstructions.Start.X, cleaningInstructions.Start.Y);
        foreach (var command in cleaningInstructions.Commands)
        {
            switch (command.Direction)
            {
                case Direction.North:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.X, yAxisCache, xAxisCache, command.Steps, step => startPosition.Y + step);
                    startPosition.Y = startPosition.Y + command.Steps;
                    break;
                case Direction.South:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.X, yAxisCache, xAxisCache, command.Steps, step => startPosition.Y - step);
                    startPosition.Y = startPosition.Y - command.Steps;
                    break;
                case Direction.East:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.Y, xAxisCache, yAxisCache, command.Steps, step => startPosition.X + step);
                    startPosition.X = startPosition.X + command.Steps;
                    break;
                case Direction.West:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.Y, xAxisCache, yAxisCache, command.Steps, step => startPosition.X - step);
                    startPosition.X = startPosition.X - command.Steps;
                    break;
                default:
                    break;
            }
        }

        return uniqueCleanedSpots;
    }

    private int ManageCleaningInAxis(int steadyPositionOnAxis,
        Dictionary<int, HashSet<int>> movingAxisCache,
        Dictionary<int, HashSet<int>> stationaryAxisCache,
        int steps,
        Func<int, int> executeMoveOperation)
    {
        var visitedPointsForStationaryAxis = stationaryAxisCache[steadyPositionOnAxis];
        var isFirstTimeVisitForAxis = visitedPointsForStationaryAxis.Count == 1;
        var uniqueCleanedSpaces = 0;

        for (int i = 1; i <= steps; i++)
        {
            var newAxisPosition = executeMoveOperation(i);

            if (isFirstTimeVisitForAxis || !visitedPointsForStationaryAxis.Contains(newAxisPosition))
            {
                uniqueCleanedSpaces++;
                visitedPointsForStationaryAxis.Add(newAxisPosition);
                if (movingAxisCache.ContainsKey(newAxisPosition))
                {
                    movingAxisCache[newAxisPosition].Add(steadyPositionOnAxis);
                }
                else
                {
                    movingAxisCache.Add(newAxisPosition, new HashSet<int>() { steadyPositionOnAxis });
                }
            }
        }
        return uniqueCleanedSpaces;
    }
}
