namespace CleaningApi.Services;

public class CleaningService
{
    public int Clean(CleaningInstructions cleaningInstructions)
    {
        var uniqueCleanedSpots = 1;
        var xAxisRangeCache = new Dictionary<int, List<(int Start, int End)>>();
        var yAxisRangeCache = new Dictionary<int, List<(int Start, int End)>>();

        (int X, int Y) startPosition = (cleaningInstructions.Start.X, cleaningInstructions.Start.Y);
        xAxisRangeCache.AddPointToCache(startPosition.X, startPosition.Y);
        yAxisRangeCache.AddPointToCache(startPosition.Y, startPosition.X);

        foreach (var command in cleaningInstructions.Commands)
        {
            switch (command.Direction)
            {
                case Direction.North:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.X, xAxisRangeCache, yAxisRangeCache, command.Steps, step => startPosition.Y + step);
                    xAxisRangeCache.AddRangeToCache(startPosition.X, (startPosition.Y, startPosition.Y + command.Steps));
                    startPosition.Y += command.Steps;
                    break;
                case Direction.South:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.X, xAxisRangeCache, yAxisRangeCache, command.Steps, step => startPosition.Y - step);
                    xAxisRangeCache.AddRangeToCache(startPosition.X, (startPosition.Y - command.Steps, startPosition.Y));
                    startPosition.Y -= command.Steps;
                    break;
                case Direction.East:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.Y, yAxisRangeCache, xAxisRangeCache, command.Steps, step => startPosition.X + step);
                    yAxisRangeCache.AddRangeToCache(startPosition.Y, (startPosition.X, startPosition.X + command.Steps));
                    startPosition.X += command.Steps;

                    break;
                case Direction.West:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.Y, yAxisRangeCache, xAxisRangeCache, command.Steps, step => startPosition.X - step);
                    yAxisRangeCache.AddRangeToCache(startPosition.Y, (startPosition.X - command.Steps, startPosition.X));
                    startPosition.X -= command.Steps;
                    break;
            }
        }

        return uniqueCleanedSpots;
    }

    private int ManageCleaningInAxis(int steadyPositionOnAxis,
        Dictionary<int, List<(int Start, int End)>> stationaryAxisCache,
        Dictionary<int, List<(int Start, int End)>> movingAxisCache,
        int steps,
        Func<int, int> executeMoveOperation)
    {
        var uniqueCleanedSpaces = 0;

        var allVisitedRanges = stationaryAxisCache[steadyPositionOnAxis];
        var isFirstTimeVisitForAxis = allVisitedRanges.Count == 1 && allVisitedRanges[0].Start == allVisitedRanges[0].End;

        for (var i = 1; i <= steps; i++)
        {
            var newAxisPosition = executeMoveOperation(i);

            if (isFirstTimeVisitForAxis || !CheckIsPointIsOnTheRange(allVisitedRanges, newAxisPosition))
            {
                uniqueCleanedSpaces++;
                movingAxisCache.AddPointToCache(newAxisPosition, steadyPositionOnAxis);
            }
        }

        return uniqueCleanedSpaces;
    }

    private bool CheckIsPointIsOnTheRange(List<(int Start, int End)> allVisitedRange, int newAxisPosition)
    {
        foreach (var range in allVisitedRange)
        {
            if (newAxisPosition >= range.Start && newAxisPosition <= range.End)
            {
                return true;
            }
        }

        return false;
    }
}