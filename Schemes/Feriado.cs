using System.ComponentModel.DataAnnotations;
namespace backend.Models;
public class Feriado
{
  [Key]
  [DataType(DataType.Date)]
  public DateOnly dia { get; set; }
  public String descricao { get; set; }
  public TipoFeriado? tipo_feriado { get; set; }
  public Regional? regional { get; set; }
  public Feriado(DateOnly dia, String descricao, Regional? regional, TipoFeriado? tipo_feriado)
  {
    this.dia = dia;
    this.descricao = descricao;
    this.tipo_feriado = tipo_feriado;
    this.regional = regional;
  }
}
