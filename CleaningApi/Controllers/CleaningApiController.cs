using CleaningApi.Services;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.Diagnostics;

namespace CleaningApi.Controllers;

[ApiController]
public class CleaningApiController(CleaningService cleaning, IExecutionRepository executionRepository) : ControllerBase
{
    private readonly CleaningService cleaning = cleaning;
    private readonly IExecutionRepository executionRepository = executionRepository;

    [HttpPost("tibber-developer-test/enter-path")]
    public async Task<CleaningResponse> Get(CleaningInstructions cleaningInstructions)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var uniqueCleanedPlaces = cleaning.Clean(cleaningInstructions);

        stopwatch.Stop();
        TimeSpan elapsed = stopwatch.Elapsed;

        var execution = new Execution { Commands = cleaningInstructions.Commands.Length, Result = uniqueCleanedPlaces, Duration = elapsed.TotalSeconds };
        await executionRepository.Save(execution);

        return new CleaningResponse(execution.Timestamp, execution.Commands, execution.Result, execution.Duration);
    }
}
