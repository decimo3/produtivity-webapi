using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace backend.Models;
public class Funcionario
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)]
  public Int32 matricula { get; set; }
  public String nome_colaborador { get; set; }
  [JsonIgnore]
  public String? palavra { get; set; }
  [JsonIgnore]
  public DateOnly? admissao { get; set; }
  [JsonIgnore]
  public DateOnly? demissao { get; set; }
  public SituacaoFuncionario situacao { get; set; } = SituacaoFuncionario.ATIVO;
  public Regional? regional { get; set; }
  public Atividade? atividade { get; set; }
  public Int32? id_superior { get; set; }
  public TipoFuncionario funcao { get; set; }
}
public enum TipoFuncionario
{
  ELETRICISTA = 0,
  SUPERVISOR = 1,
  ADMINISTRATIVO = 2,
}
public enum SituacaoFuncionario
{
  ATIVO = 0,
  DESLIGADO = 1,
  FERIAS = 2,
  AFASTADO = 3

}
