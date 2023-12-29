namespace CleaningApi;

public class CleaningResponse
{
    public int Id = 0;
    public DateTime Timestamp { get; }

    public int Commands { get; }

    public int Result { get; }

    public double Duration { get; }

    public CleaningResponse(DateTime timestamp, int commands, int result, double duration)
    {
        Timestamp = timestamp;
        Commands = commands;
        Result = result;
        Duration = duration;
    }
}
