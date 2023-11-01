namespace backend.Models;
public class ErroValidacao
{
  public int linha { get; set; }
  public string? campo { get; set; }
  public string? valor { get; set; }
  public string mensagem { get; set; }
  public ErroValidacao(int linha, string? campo, string? valor, string mensagem)
  {
    this.linha = linha;
    this.campo = campo;
    this.valor = valor;
    this.mensagem = mensagem;
  }
}
