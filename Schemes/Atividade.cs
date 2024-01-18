using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;
public class Atividade
{
  public Int16 id_processo { get; set; }
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)]
  public Int16 id_atividade { get; set; }
  public String atividade { get; set; }

  public Atividade (Int16 id_processo, Int16 id_atividade, String atividade)
  {
    this.id_processo = id_processo;
    this.id_atividade = id_atividade;
    this.atividade = atividade;
  }
}
