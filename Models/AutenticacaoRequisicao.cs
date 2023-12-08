using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace backend.Models;
public class AutenticacaoRequisicao
{
  [Required(ErrorMessage = "Necessário informar a matricula!")]
  [RegularExpression(pattern: "^[0-9]{7}$", ErrorMessage = "Matrícula inválida!")]
  public Int32 matricula { get; set; }
  [Required(ErrorMessage = "Necessário informar a senha!")]
  public String palavra { get; set; }
  public AutenticacaoRequisicao(Int32 matricula, String palavra)
  {
    this.matricula = matricula;
    this.palavra = palavra;
  }
}
