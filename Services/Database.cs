using backend.Models;
using Microsoft.EntityFrameworkCore;
namespace backend.Services;
public class Database : DbContext
{
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if(System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    {
      optionsBuilder.UseSqlite("Data Source=AppData/database.db")
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
    }
    else
    {
      var server = System.Environment.GetEnvironmentVariable("MYSQL_HOST");
      if(server is null) throw new InvalidOperationException("Environment variable MYSQL_HOST is not set!");
      var dbuser = System.Environment.GetEnvironmentVariable("MYSQL_USER");
      if(dbuser is null) throw new InvalidOperationException("Environment variable MYSQL_USER is not set!");
      var dbpass = System.Environment.GetEnvironmentVariable("MYSQL_PASSWORD");
      if(dbpass is null) throw new InvalidOperationException("Environment variable MYSQL_PASSWORD is not set!");
      var dbbase = System.Environment.GetEnvironmentVariable("MYSQL_DATABASE");
      if(dbbase is null) throw new InvalidOperationException("Environment variable MYSQL_DATABASE is not set!");
      var stringconnection = $"server={server};user={dbuser};password={dbpass};database={dbbase}";
      var serverVersion = new MySqlServerVersion(new Version(8, 0, 34));
      optionsBuilder.UseMySql(stringconnection, serverVersion);
    }
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // TODO: Implement database modeling
    modelBuilder.Entity<Composicao>().HasKey(o => new {o.recurso, o.dia});
  }
  public DbSet<Composicao> composicao { get; set; }
  public DbSet<Servico> relatorio { get; set; }
  public DbSet<Funcionario> funcionario { get; set; }
}
