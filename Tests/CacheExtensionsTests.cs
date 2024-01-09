using AutoFixture;
using CleaningApi;
using FluentAssertions;

namespace Tests;

public class CacheExtensionsTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void AddPointToEmptyCache_AddsThePoint()
    {
        var r = new System.Range(10, 10);
        // Arrange
        var cacheKey = _fixture.Create<int>();
        var cache = new Dictionary<int, List<PointRange>>();

        // Act
        cache.AddPointToCache(cacheKey, 3);

        (3, 3).Should().Be((3, 3));

        // Assert
        cache[cacheKey].Should().BeEquivalentTo([new PointRange(3, 3)]);
    }

    [Fact]
    public void AddPointOutsideOfExistingRange_AddsThePoint()
    {
        // Arrange
        var cacheKey = _fixture.Create<int>();
        var cache = new Dictionary<int, List<PointRange>>();
        cache.Add(cacheKey, [new PointRange(3, 8)]);

        // Act
        cache.AddPointToCache(cacheKey, 10);

        // Assert
        cache[cacheKey].Should().BeEquivalentTo([new PointRange(3, 8), new PointRange(10, 10)]);
    }

    [Fact]
    public void AddPointWithinAnExistingRange_DoesNotAddThePoint()
    {
        // Arrange
        var cacheKey = _fixture.Create<int>();
        var cache = new Dictionary<int, List<PointRange>>();
        cache.Add(cacheKey, [new PointRange(3, 8)]);

        // Act
        cache.AddPointToCache(cacheKey, 3);

        // Assert
        cache[cacheKey].Should().BeEquivalentTo([new PointRange(3, 8)]);
    }

    [Fact]
    public void AddPointThatIsAtTheBeginningOfAnExistingRange_ExtendsTheRange()
    {
        // Arrange
        var cacheKey = _fixture.Create<int>();
        var cache = new Dictionary<int, List<PointRange>>();
        cache.Add(cacheKey, [new PointRange(3, 8)]);

        // Act
        cache.AddPointToCache(cacheKey, 2);

        // Assert
        cache[cacheKey].Should().BeEquivalentTo([new PointRange(2, 8)]);
    }

    [Fact]
    public void AddPointThatIsAtTheEndOfAnExistingRange_ExtendsTheRange()
    {
        // Arrange
        var cacheKey = _fixture.Create<int>();
        var cache = new Dictionary<int, List<PointRange>>();
        cache.Add(cacheKey, [new PointRange(3, 8)]);

        // Act
        cache.AddPointToCache(cacheKey, 9);

        // Assert
        cache[cacheKey].Should().BeEquivalentTo([new PointRange(3, 9)]);
    }

    [Fact]
    public void AddRangeToEmptyCache_AddsTheRange()
    {
        // Arrange
        var cacheKey = _fixture.Create<int>();
        var cache = new Dictionary<int, List<PointRange>>();

        // Act
        cache.AddRangeToCache(cacheKey, (2, 3));

        (3, 3).Should().Be((3, 3));

        // Assert
        cache[cacheKey].Should().BeEquivalentTo([new PointRange(2, 3)]);
    }

    [Fact]
    public void AddRangeOutsideOfExistingRange_AddsTheRange()
    {
        // Arrange
        var cacheKey = _fixture.Create<int>();
        var cache = new Dictionary<int, List<PointRange>>();
        cache.Add(cacheKey, [new PointRange(3, 8)]);

        // Act
        cache.AddRangeToCache(cacheKey, (10, 13));

        // Assert
        cache[cacheKey].Should().BeEquivalentTo([new PointRange(3, 8), new PointRange(10, 13)]);
    }

    [Fact]
    public void AddRangeWithinExistingRange_DoesNotAddTheRange()
    {
        // Arrange
        var cacheKey = _fixture.Create<int>();
        var cache = new Dictionary<int, List<PointRange>>();
        cache.Add(cacheKey, [new PointRange(3, 8)]);

        // Act
        cache.AddRangeToCache(cacheKey, (3, 6));

        // Assert
        cache[cacheKey].Should().BeEquivalentTo([new PointRange(3, 8)]);
    }
}