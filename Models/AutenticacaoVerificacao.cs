namespace backend.Models;
public class AutenticacaoVerificacao
{
  public Int32 matricula { get; set; }
  public DateOnly admissao { get; set; }
  public String nome_colaborador { get; set; }
  public AutenticacaoVerificacao(Int32 matricula, DateOnly admissao, String nome_colaborador)
  {
    this.matricula = matricula;
    this.admissao = admissao;
    this.nome_colaborador = nome_colaborador;
  }
}
