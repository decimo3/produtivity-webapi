namespace backend.Models;
public class AutenticacaoTrocarSenha
{
  public String? atual { get; set; }
  public String nova { get; set; }
  public String confirmacao { get; set; }
  public AutenticacaoTrocarSenha(String? atual, String nova, String confirmacao)
  {
    this.atual = atual;
    this.nova = nova;
    this.confirmacao = confirmacao;
  }
}
