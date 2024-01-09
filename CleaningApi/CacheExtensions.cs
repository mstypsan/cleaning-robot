namespace CleaningApi;

public static class CacheExtensions
{
    public static void AddRangeToCache(this Dictionary<int, List<(int Start, int End)>> cache, int key, (int Start, int End) newRange)
    {
        if (!cache.TryGetValue(key, out var availableRanges))
        {
            cache.Add(key, [newRange]);
            return;
        }

        var shouldBeAddedToCache = true;

        foreach (var range in availableRanges)
        {
            if (range.Start <= newRange.Start && range.End >= newRange.End)
            {
                shouldBeAddedToCache = false;
                break;
            }

            if (newRange.Start < range.Start && newRange.End >= range.End || newRange.Start <= range.Start && newRange.End > range.End)
            {
                availableRanges.Remove(range);
                break;
            }
        }

        if (shouldBeAddedToCache)
        {
            availableRanges.Add(newRange);
        }
    }

    public static void AddPointToCache(this Dictionary<int, List<(int Start, int End)>> cache, int key, int point)
    {
        var newRange = (point, point);
        if (!cache.TryGetValue(key, out var availableRanges))
        {
            cache.Add(key, [newRange]);
            return;
        }

        foreach (var range in availableRanges)
        {
            if (point >= range.Start && point <= range.End)
            {
                return;
            }

            if (point == range.Start - 1)
            {
                availableRanges.Remove(range);
                availableRanges.Add((range.Start - 1, range.End));
                return;
            }

            if (point == range.End + 1)
            {
                availableRanges.Remove(range);
                availableRanges.Add((range.Start, range.End + 1));
                return;
            }
        }

        cache[key].Add(newRange);
    }
}