namespace CleaningApi;

public record CleaningInstructions(StartCoordinate Start, Command[] Commands);

public record StartCoordinate(int X, int Y);

public record Command(Direction Direction, int Steps);
