using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models;
public class Valoracao
{
  public Int64? contrato { get; set; }
  public Int16? revisao { get; set; }
  public Regional regional { get; set; }
  public TipoViatura tipo_viatura { get; set; }
  public Atividade atividade { get; set; }
  public String codigo { get; set; }
  [Column(TypeName="money")]
  public Decimal valor { get; set; }
  public Valoracao(Regional regional, TipoViatura tipo_viatura, Atividade atividade, String codigo, Decimal valor, Int64? contrato = null, Int16? revisao = null)
  {
    this.regional = regional;
    this.tipo_viatura = tipo_viatura;
    this.atividade = atividade;
    this.codigo = codigo;
    this.valor = valor;
    this.contrato = contrato;
    this.revisao = revisao;
  }
}
