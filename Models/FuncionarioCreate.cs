using System.Text.Json.Serialization;

namespace backend.Models;
public class FuncionarioCreate
{
  public Int32 matricula { get; set; }
  public String nome_colaborador { get; set; }
  public DateOnly admissao { get; set; }
  public TipoFuncionario funcao { get; set; }
  public Regional regional { get; set; }
  public Atividade atividade { get; set; }
  public SituacaoFuncionario situacao { get; set; } = SituacaoFuncionario.ATIVO;
  [JsonConstructor]
  public FuncionarioCreate(Int32 matricula, String nome_colaborador, TipoFuncionario funcao, DateOnly admissao, Regional regional, Atividade atividade)
  {
    this.nome_colaborador = nome_colaborador;
    this.matricula = matricula;
    this.funcao = funcao;
    this.admissao = admissao;
    this.regional = regional;
    this.atividade = atividade;
  }
}
