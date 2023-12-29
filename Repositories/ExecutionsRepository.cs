namespace Repositories;

public class ExecutionsRepository(ExecutionsContext databaseContext) : IExecutionRepository
{
    private readonly ExecutionsContext databaseContext = databaseContext;

    public async Task Save(Execution execution)
    {
        databaseContext.Executions.Add(execution);
        await databaseContext.SaveChangesAsync();
    }
}
