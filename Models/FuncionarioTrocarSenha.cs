namespace backend.Models;
public class FuncionarioTrocarSenha
{
  public String? atual { get; set; }
  public String nova { get; set; }
  public String confirmacao { get; set; }
  public FuncionarioTrocarSenha(String? atual, String nova, String confirmacao)
  {
    this.atual = atual;
    this.nova = nova;
    this.confirmacao = confirmacao;
  }
}
