using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models;
public class Composicao
{
  [Required]
  public DateOnly dia { get; set; }
  [Required]
  [Range(11400, 17000, ErrorMessage = "O valor do adesivo não é válido!")]
  public int adesivo { get; set; }
  [Required]
  public string? placa { get; set; }
  [Required]
  [StringLength(32, ErrorMessage = "O nome do recurso é muito longo!")]
  public string recurso { get; set; }
  [Required]
  public Atividade atividade { get; set; }
  [Required]
  public string? motorista { get; set; }
  [Required]
  [RegularExpression("^[0-9]{7}$")]
  public int id_motorista { get; set; }
  [Required]
  public string? ajudante { get; set; }
  [Required]
  [RegularExpression("^[0-9]{7}$")]
  public int id_ajudante { get; set; }
  [Required]
  public Int64 telefone { get; set; }
  [Required]
  [RegularExpression("^[0-9]{7}$")]
  public int id_supervisor { get; set; }
  [Required]
  public string? supervisor { get; set; }
  [Required]
  public Regional regional { get; set; }
  [NotMapped]
  public List<string> validacao { get; set; }
  public Composicao()
  {
    this.recurso = "";
    this.validacao = new();
  }
}
