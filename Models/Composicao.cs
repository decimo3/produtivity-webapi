using System.ComponentModel.DataAnnotations;
namespace backend.Models;
public class Composicao : IEntity, IValidatableObject
{
  [Required]
  public DateOnly dia { get; set; }
  [Required]
  [Range(11400, 17000, ErrorMessage = "O valor do adesivo não é válido!")]
  public int adesivo { get; set; }
  [Required]
  [RegularExpression("^[A-z0-9]{7}$", ErrorMessage = "A placa informada não é valida ou não está escrita no padrão (AAA-BBBB)!")]
  public string placa { get; set; }
  [Required]
  [StringLength(32, ErrorMessage = "O nome do recurso é muito longo!")]
  public string recurso { get; set; }
  [Required]
  public Atividade atividade { get; set; }
  [Required]
  public string motorista { get; set; }
  [Required]
  [RegularExpression("^[0-9]{7}$")]
  public int id_motorista { get; set; }
  [Required]
  public string ajudante { get; set; }
  [Required]
  [RegularExpression("^[0-9]{7}$")]
  public int id_ajudante { get; set; }
  [Required]
  public Int64 telefone { get; set; }
  [Required]
  [RegularExpression("^[0-9]{7}$")]
  public int id_supervisor { get; set; }
  [Required]
  public string supervisor { get; set; }
  [Required]
  public Regional regional { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    var results = new List<ValidationResult>();
    return results;
  }
}
