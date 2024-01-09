using CleaningApi;
using CleaningApi.Services;
using FluentAssertions;

namespace Tests;

public class CleaningServiceTests
{
    [Theory]
    [InlineData(2, 3, new[] { Direction.North }, new[] { 10 }, 11)]
    [InlineData(2, 3, new[] { Direction.West }, new[] { 10 }, 11)]
    [InlineData(2, 3, new[] { Direction.East }, new[] { 10 }, 11)]
    [InlineData(2, 3, new[] { Direction.South }, new[] { 10 }, 11)]
    [InlineData(2, 3, new[] { Direction.North, Direction.South }, new[] { 10, 5 }, 11)]
    [InlineData(2, 3, new[] { Direction.West, Direction.East }, new[] { 10, 5 }, 11)]
    [InlineData(2, 3, new[] { Direction.West, Direction.North }, new[] { 10, 5 }, 16)]
    [InlineData(2, 3, new[] { Direction.West, Direction.North, Direction.East, Direction.South }, new[] { 10, 5, 10, 6 }, 31)]
    [InlineData(-2, -3, new[] { Direction.West, Direction.North, Direction.East, Direction.South }, new[] { 10, 5, 10, 6 }, 31)]
    [InlineData(-2, -3, new[] { Direction.West, Direction.North, Direction.East, Direction.South, Direction.West, Direction.North, Direction.East, Direction.South },
        new[] { 3, 3, 3, 3, 3, 3, 3, 3 }, 12)]
    [InlineData(2, 30, new[] { Direction.West, Direction.North, Direction.East, Direction.South, Direction.North, Direction.West }, new[] { 10, 5, 10, 6, 1, 3 }, 31)]
    [InlineData(-2, -30,
        new[]
        {
            Direction.West, Direction.North, Direction.East, Direction.South, Direction.North, Direction.West
        }, new[] { 10, 5, 10, 6, 1, 3 }, 31)]
    [InlineData(0, 0,
        new[]
        {
            Direction.West, Direction.North, Direction.East, Direction.South, Direction.West, Direction.North, Direction.East, Direction.South, Direction.West, Direction.North,
            Direction.East, Direction.South
        }, new[] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 5 }, 42)]
    public void ForGivenCleaningInstruction_ReturnUniqueSpots(int x, int y, Direction[] directions, int[] steps, int expectedUniqueSteps)
    {
        // Arrange
        var cleaningService = new CleaningService();
        var commands = new List<Command>();
        for (int i = 0; i < directions.Length; i++)
        {
            commands.Add(new Command(directions[i], steps[i]));
        }

        var cleaningInstructions = new CleaningInstructions(new StartCoordinate(x, y), commands.ToArray());

        // Act
        var uniqueCleaningPoints = cleaningService.Clean(cleaningInstructions);

        // Assert
        uniqueCleaningPoints.Should().Be(expectedUniqueSteps);
    }
}