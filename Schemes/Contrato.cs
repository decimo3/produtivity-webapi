using System.ComponentModel.DataAnnotations;
namespace backend.Models;
public class Contrato
{
  [Key]
  public String contrato { get; set; }
  [DataType(DataType.Date)]
  public DateOnly inicio_vigencia { get; set; }
  [DataType(DataType.Date)]
  public DateOnly final_vigencia { get; set; }
  public Regional regional { get; set; }
  public Contrato(String contrato, DateOnly inicio_vigencia, DateOnly final_vigencia, Regional regional)
  {
    this.contrato = contrato;
    this.inicio_vigencia = inicio_vigencia;
    this.final_vigencia = final_vigencia;
    this.regional = regional;
  }
}
