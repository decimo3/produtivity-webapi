using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models;
public class Valoracao
{
  public String? contrato { get; set; }
  public Regional regional { get; set; }
  public TipoViatura tipo_viatura { get; set; }
  public Atividade atividade { get; set; }
  public String codigo { get; set; }
  [Column(TypeName="money")]
  public Decimal valor { get; set; }
  public Valoracao(Regional regional, TipoViatura tipo_viatura, Atividade atividade, String codigo, Decimal valor, String? contrato = null)
  {
    this.regional = regional;
    this.tipo_viatura = tipo_viatura;
    this.atividade = atividade;
    this.codigo = codigo;
    this.valor = valor;
    this.contrato = contrato;
  }
}
