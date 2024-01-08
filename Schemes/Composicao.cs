using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend.Models;
public class Composicao
{
  public String identificador { get; set; }
  [DataType(DataType.Date)]
  public DateOnly dia { get; set; }
  public int? adesivo { get; set; } = 0;
  public string? placa { get; set; } = "";
  public string recurso { get; set; }
  public Atividade atividade { get; set; }
  public string? motorista { get; set; }
  public int id_motorista { get; set; }
  public string? ajudante { get; set; }
  public int id_ajudante { get; set; }
  public Int64 telefone { get; set; } = 0;
  public int id_supervisor { get; set; }
  public string? supervisor { get; set; }
  public Regional regional { get; set; }
  public TipoViatura tipo_viatura { get; set; }
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
  EMERGENCIA = 5,
  ESTOQUE = 6
}
public enum Regional
{
  NENHUM = 0,
  BAIXADA = 1,
  OESTE = 2
}
public enum TipoViatura
{
  NENHUM = 0,
  LEVE = 1,
  PESADO = 2
}
