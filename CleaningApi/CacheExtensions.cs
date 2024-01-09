using CleaningApi.Services;

namespace CleaningApi;

public static class CacheExtensions
{
    public static void AddRangeToCache(this Dictionary<int, List<PointRange>> cache, int key, (int Start, int End) newRange)
    {
        if (!cache.TryGetValue(key, out var availableRanges))
        {
            cache.Add(key, [new PointRange(newRange.Start, newRange.End)]);
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
            availableRanges.Add(new PointRange(newRange.Start, newRange.End));
        }
    }

    public static void AddPointToCache(this Dictionary<int, List<PointRange>> cache, int key, int point)
    {
        if (!cache.TryGetValue(key, out var availableRanges))
        {
            cache.Add(key, [new PointRange(point, point)]);
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
                range.Start -= 1;
                return;
            }

            if (point == range.End + 1)
            {
                range.End += 1;
                return;
            }
        }

        cache[key].Add(new PointRange(point, point));
    }
}