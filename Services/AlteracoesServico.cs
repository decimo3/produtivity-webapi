namespace backend.Services;
using backend.Models;
public class AlteracoesServico
{
  private readonly Database database;
  public Int32 responsavel { get; set; }
  public String tabela { get; set; }
  public AlteracoesServico(Database database)
  {
    this.database = database;
    this.responsavel = 0;
    this.tabela = String.Empty;
  }
  public void Registrar(String verbo, Object? valorAnterior, Object? valorPosterior)
  {
    var jsonAnterior = System.Text.Json.JsonSerializer.Serialize(valorAnterior) ?? String.Empty;
    var jsonPosterior = System.Text.Json.JsonSerializer.Serialize(valorPosterior) ?? String.Empty;
    if(responsavel == 0 || tabela == String.Empty) throw new InvalidOperationException("O log não pode ser registrado antes de ser configurado!");
    var alteracao = new Alteracao(responsavel, tabela, verbo, jsonAnterior, jsonPosterior);
    var console = $"Em {alteracao.timestamp} o id {alteracao.responsavel} fez {alteracao.verbo} na {alteracao.tabela}: {alteracao.valorAnterior} para {alteracao.valorPosterior}.";
    this.database.alteracao.Add(alteracao);
    this.database.SaveChanges();
    System.Console.WriteLine(console);
  }
  public IEnumerable<Alteracao> Verificar(Int32 pagina)
  {
    var minimo = pagina * 100;
    var maximo = minimo + 100;
    return this.database.alteracao
      .OrderBy(a => a.identificador)
      .Where(o => o.identificador > minimo && o.identificador < maximo)
      .ToList();
  }
  public bool is_ready()
  {
    if(this.database == null) return false;
    if(this.responsavel == 0) return false;
    if(this.tabela == String.Empty) return false;
    return true;
  }
}
