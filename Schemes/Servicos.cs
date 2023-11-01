using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;
public class Servico : IEntity, IValidatableObject
{
  public string recurso { get; set; }
  public DateOnly dia { get; set; }
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)]
  public int indentificador { get; set; }
  public string status { get; set; }
  public TimeOnly? hora_inicio { get; set; }
  public TimeOnly? hora_final { get; set; }
  public TimeOnly? duracao_feito { get; set; }
  public TimeOnly? desloca_feito { get; set; }
  public string? tipo_atividade { get; set; }
  public long servico { get; set; }
  public string? codigos { get; set; }
  public string? observacao { get; set; }
  public int? instalacao { get; set; }
  public string tipo_servico { get; set; }
  public TimeOnly? Desloca_estima { get; set; }
  public TimeOnly? duracao_estima { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    throw new NotImplementedException();
  }
}
