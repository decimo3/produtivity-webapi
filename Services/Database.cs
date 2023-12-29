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
      var dbhost = System.Environment.GetEnvironmentVariable("PGHOST");
      if(dbhost is null) throw new InvalidOperationException("Environment variable PGHOST is not set!");
      var dbport = System.Environment.GetEnvironmentVariable("PGPORT");
      if(dbport is null) throw new InvalidOperationException("Environment variable PGPORT is not set!");
      var dbuser = System.Environment.GetEnvironmentVariable("PGUSER");
      if(dbuser is null) throw new InvalidOperationException("Environment variable PGUSER is not set!");
      var dbpass = System.Environment.GetEnvironmentVariable("PGPASSWORD");
      if(dbpass is null) throw new InvalidOperationException("Environment variable PGPASSWORD is not set!");
      var dbbase = System.Environment.GetEnvironmentVariable("PGDATABASE");
      if(dbbase is null) throw new InvalidOperationException("Environment variable PGDATABASE is not set!");
      var stringconnection = new Npgsql.NpgsqlConnectionStringBuilder()
      {
        Host = dbhost,
        Port = Int32.Parse(dbport),
        Username = dbuser,
        Password = dbpass,
        Database = dbbase
      };
      optionsBuilder.UseNpgsql(stringconnection.ConnectionString);
    }
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Composicao>().HasKey(o => new {o.dia, o.recurso});
    modelBuilder.Entity<Objetivos>().HasKey(o => new {o.regional, o.tipo_viatura, o.atividade});
    modelBuilder.Entity<Valoracao>().HasKey(o => new {o.regional, o.tipo_viatura, o.atividade, o.codigo});
  }
  public DbSet<Composicao> composicao { get; set; }
  public DbSet<Servico> relatorio { get; set; }
  public DbSet<Funcionario> funcionario { get; set; }
  public DbSet<Valoracao> valoracao { get; set; }
  public DbSet<Objetivos> objetivo { get; set; }
  public DbSet<Contrato> contrato { get; set; }
  public DbSet<Feriado> feriado { get; set; }
  public DbSet<RelatorioEstatisticas> relatorioEstatisticas { get; set; }
}
