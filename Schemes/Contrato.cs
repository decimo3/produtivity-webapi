using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models;
public class Contrato
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)]
  public Int64 contrato { get; set; }
  [DataType(DataType.Date)]
  public DateOnly inicio_vigencia { get; set; }
  [DataType(DataType.Date)]
  public DateOnly final_vigencia { get; set; }
  public Regional regional { get; set; }
  public Atividade atividade { get; set; }
  public Int16 revisao { get; set; }
  public Contrato(Int64 contrato, DateOnly inicio_vigencia, DateOnly final_vigencia, Regional regional, Int16 revisao, Atividade atividade)
  {
    this.contrato = contrato;
    this.inicio_vigencia = inicio_vigencia;
    this.final_vigencia = final_vigencia;
    this.regional = regional;
    this.atividade = atividade;
    this.revisao = revisao;
  }
}
