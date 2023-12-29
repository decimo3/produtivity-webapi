using System.ComponentModel.DataAnnotations;
namespace backend.Models;
public class RelatorioEstatisticas
{
  [Key]
  public String filename { get; set; }
  [DataType(DataType.Date)]
  public DateOnly dia { get; set; }
  public Int32 recursos { get; set; }
  public Int32 servicos { get; set; }
  public RelatorioEstatisticas(String filename, DateOnly dia, Int32 recursos, Int32 servicos)
  {
    this.filename = filename;
    this.dia = dia;
    this.recursos = recursos;
    this.servicos = servicos;
  }
}
