using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models;
public class Alteracao
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public Int64 identificador { get; set; }
  [DataType(DataType.DateTime)]
  public DateTime timestamp { get; set; }
  public Int32 responsavel { get; set; }
  public String tabela { get; set; }
  public String verbo { get; set; }
  public String? valorAnterior { get; set; }
  public String? valorPosterior { get; set; }
  public Alteracao(Int32 responsavel, String tabela, String verbo, String valorAnterior, String valorPosterior)
  {
    this.timestamp = DateTime.Now.ToUniversalTime();
    this.responsavel = responsavel;
    this.tabela = tabela;
    this.verbo = verbo;
    this.valorAnterior = valorAnterior;
    this.valorPosterior = valorPosterior;
  }
}
