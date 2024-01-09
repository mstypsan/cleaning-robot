namespace CleaningApi.Services;

public class CleaningService
{
    public int Clean(CleaningInstructions cleaningInstructions)
    {
        var uniqueCleanedSpots = 1;
        var xAxisRangeCache = new Dictionary<int, List<PointRange>>();
        var yAxisRangeCache = new Dictionary<int, List<PointRange>>();

        (int X, int Y) startPosition = (cleaningInstructions.Start.X, cleaningInstructions.Start.Y);
        xAxisRangeCache.AddPointToCache(startPosition.X, startPosition.Y);
        yAxisRangeCache.AddPointToCache(startPosition.Y, startPosition.X);

        foreach (var command in cleaningInstructions.Commands)
        {
            switch (command.Direction)
            {
                case Direction.North:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.X, xAxisRangeCache, yAxisRangeCache, (startPosition.Y + 1, startPosition.Y + command.Steps));
                    xAxisRangeCache.AddRangeToCache(startPosition.X, (startPosition.Y, startPosition.Y + command.Steps));
                    startPosition.Y += command.Steps;
                    break;
                case Direction.South:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.X, xAxisRangeCache, yAxisRangeCache, (startPosition.Y - command.Steps, startPosition.Y - 1));
                    xAxisRangeCache.AddRangeToCache(startPosition.X, (startPosition.Y - command.Steps, startPosition.Y));
                    startPosition.Y -= command.Steps;
                    break;
                case Direction.East:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.Y, yAxisRangeCache, xAxisRangeCache, (startPosition.X + 1, startPosition.X + command.Steps));
                    yAxisRangeCache.AddRangeToCache(startPosition.Y, (startPosition.X, startPosition.X + command.Steps));
                    startPosition.X += command.Steps;
                    break;
                case Direction.West:
                    uniqueCleanedSpots += ManageCleaningInAxis(startPosition.Y, yAxisRangeCache, xAxisRangeCache, (startPosition.X - command.Steps, startPosition.X - 1));
                    yAxisRangeCache.AddRangeToCache(startPosition.Y, (startPosition.X - command.Steps, startPosition.X));
                    startPosition.X -= command.Steps;
                    break;
            }
        }

        return uniqueCleanedSpots;
    }

    private int ManageCleaningInAxis(int steadyPositionOnAxis,
        Dictionary<int, List<PointRange>> stationaryAxisCache,
        Dictionary<int, List<PointRange>> movingAxisCache,
        (int Start, int End) newMovingRange)
    {
        var uniqueCleanedSpaces = 0;

        var allVisitedRanges = stationaryAxisCache[steadyPositionOnAxis];
        var isFirstTimeVisitForAxis = allVisitedRanges.Count == 1 && allVisitedRanges[0].Start == allVisitedRanges[0].End;
        var hasAlreadyVisitedRange = IsRangeIncludedInVisitedRanges(allVisitedRanges, newMovingRange);
        if (hasAlreadyVisitedRange)
        {
            return 0;
        }

        for (var newAxisPosition = newMovingRange.Start; newAxisPosition <= newMovingRange.End; newAxisPosition++)
        {
            if (isFirstTimeVisitForAxis || !IsPointInVisitedRanges(allVisitedRanges, newAxisPosition))
            {
                uniqueCleanedSpaces++;
                movingAxisCache.AddPointToCache(newAxisPosition, steadyPositionOnAxis);
            }
        }

        return uniqueCleanedSpaces;
    }

    private bool IsPointInVisitedRanges(List<PointRange> allVisitedRanges, int newAxisPosition)
    {
        foreach (var range in allVisitedRanges)
        {
            if (newAxisPosition >= range.Start && newAxisPosition <= range.End)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsRangeIncludedInVisitedRanges(List<PointRange> allVisitedRanges, (int Start, int End) newRange)
    {
        foreach (var range in allVisitedRanges)
        {
            if (newRange.Start >= range.Start && newRange.End <= range.End)
            {
                return true;
            }
        }

        return false;
    }
}