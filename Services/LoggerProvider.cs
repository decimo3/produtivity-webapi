namespace backend.Services;
public class LoggerDatabaseProvider : ILoggerProvider
{
  public readonly Database database;
  public LoggerDatabaseProvider(Database database)
  {
    this.database = database;
  }
  public ILogger CreateLogger(string categoryName)
  {
    throw new NotImplementedException();
  }

  public void Dispose()
  {
    throw new NotImplementedException();
  }
}
