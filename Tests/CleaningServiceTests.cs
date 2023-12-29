using CleaningApi;
using CleaningApi.Services;
using FluentAssertions;

namespace Tests;

public class CleaningServiceTests
{
    [Theory]
    [InlineData(2, 3, new Direction[] { Direction.North }, new int[] { 10 }, 11)]
    [InlineData(2, 3, new Direction[] { Direction.West }, new int[] { 10 }, 11)]
    [InlineData(2, 3, new Direction[] { Direction.East }, new int[] { 10 }, 11)]
    [InlineData(2, 3, new Direction[] { Direction.South }, new int[] { 10 }, 11)]
    [InlineData(2, 3, new Direction[] { Direction.North, Direction.South }, new int[] { 10, 5 }, 11)]
    [InlineData(2, 3, new Direction[] { Direction.West, Direction.East }, new int[] { 10, 5 }, 11)]

    [InlineData(2, 3, new Direction[] { Direction.West, Direction.North }, new int[] { 10, 5 }, 16)]
    [InlineData(2, 3, new Direction[] { Direction.West, Direction.North, Direction.East, Direction.South }, new int[] { 10, 5, 10, 6 }, 31)]
    [InlineData(-2, -30, new Direction[] { Direction.West, Direction.North, Direction.East, Direction.South, Direction.North, Direction.West }, new int[] { 10, 5, 10, 6, 1, 3 }, 31)]
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
