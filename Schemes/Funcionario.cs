using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;
public class Funcionario
{
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)]
  public int matricula { get; set; }
  [Required]
  public string nome_colaborador { get; set; }
  public TipoFuncionario funcao { get; set; }
}
public enum TipoFuncionario
{
  ELETRICISTA = 0,
  SUPERVISOR = 1
}
