using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models;
[Index(nameof(identificador), IsUnique = true)]
public class Composicao
{
  public String identificador { get; set; }
  [DataType(DataType.Date)]
  public DateOnly dia { get; set; }
  public int? adesivo { get; set; }
  public string? placa { get; set; }
  public string recurso { get; set; }
  public Atividade atividade { get; set; }
  public string? motorista { get; set; }
  public int id_motorista { get; set; }
  public string? ajudante { get; set; }
  public int id_ajudante { get; set; }
  public Int64 telefone { get; set; }
  public int id_supervisor { get; set; }
  public string? supervisor { get; set; }
  public Regional regional { get; set; }
  public TipoViatura tipo_viatura { get; set; }
  [NotMapped]
  public List<string> validacao { get; set; }
  public String? abreviacao { get; set; }
  public Int64? contrato { get; set; }
  public Int16? revisao { get; set; }
}
