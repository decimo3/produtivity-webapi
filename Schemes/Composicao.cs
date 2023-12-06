using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models;
public class Composicao
{
  [Required]
  public DateOnly dia { get; set; }
  [Required]
  public int adesivo { get; set; }
  [Required]
  public string? placa { get; set; }
  [Required]
  public string recurso { get; set; }
  [Required]
  public Atividade atividade { get; set; }
  [Required]
  public string? motorista { get; set; }
  [Required]
  public int id_motorista { get; set; }
  [Required]
  public string? ajudante { get; set; }
  [Required]
  public int id_ajudante { get; set; }
  [Required]
  public Int64 telefone { get; set; }
  [Required]
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
public enum Atividade {
  NENHUM = 0,
  CORTE = 1,
  RELIGA = 2,
  AVANCADO = 3,
  CAMINHAO = 4,
  EMERGENCIA = 5
}
public enum Regional
{
  NENHUM = 0,
  BAIXADA = 1,
  OESTE = 2
}
