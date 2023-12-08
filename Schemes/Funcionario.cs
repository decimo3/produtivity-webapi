using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace backend.Models;
public class Funcionario
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)]
  public int matricula { get; set; }
  [Required]
  public string nome_colaborador { get; set; }
  [JsonIgnore]
  public String palavra { get; set; }
  public TipoFuncionario funcao { get; set; }
}
public enum TipoFuncionario
{
  ELETRICISTA = 0,
  SUPERVISOR = 1
}
