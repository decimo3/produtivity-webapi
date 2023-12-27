using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models;
public class Objetivos
{
  public String? contrato { get; set; }
  public Regional regional { get; set; }
  public TipoViatura tipo_viatura { get; set; }
  public Atividade atividade { get; set; }
  [Column(TypeName="money")]
  public Decimal meta_producao { get; set; }
  public Int32 meta_apresentacao { get; set; }
  public Int32 meta_apresentacao_feriado { get; set; }
  public Int32 meta_execucoes { get; set; }
  public Objetivos(Regional regional, TipoViatura tipo_viatura, Atividade atividade, Decimal meta_producao, Int32 meta_apresentacao, Int32 meta_apresentacao_feriado, Int32 meta_execucoes, String? contrato = null)
  {
    this.regional = regional;
    this.tipo_viatura = tipo_viatura;
    this.atividade = atividade;
    this.meta_producao = meta_producao;
    this.meta_apresentacao = meta_apresentacao;
    this.meta_apresentacao_feriado = meta_apresentacao_feriado;
    this.meta_execucoes = meta_execucoes;
    this.contrato = contrato;
  }
}
