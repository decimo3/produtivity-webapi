using System.ComponentModel.DataAnnotations;
namespace backend.Models;
public class Feriado
{
  [Key]
  [DataType(DataType.Date)]
  public DateOnly dia { get; set; }
  public String descricao { get; set; }
  public TipoFeriado tipo_feriado { get; set; }
  public Regional regional { get; set; }
  public Feriado(DateOnly dia, String descricao, TipoFeriado tipo_feriado = 0, Regional regional = 0)
  {
    this.dia = dia;
    this.descricao = descricao;
    this.tipo_feriado = tipo_feriado;
    this.regional = regional;
  }
}
public enum TipoFeriado
{
  NENHUM = 0,
  ANUAL = 1,
  ISOLADO = 2,
  NACIONAL = 3,
  ESTADUAL = 4,
  MUNINCIPAL = 5
}
