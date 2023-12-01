using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;
public class Servico
{
  public String filename { get; set; }
  public String recurso { get; set; }
  public DateOnly dia { get; set; }
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)]
  public Int64 indentificador { get; set; }
  public TipoStatus status { get; set; }
  public String? nome_do_cliente { get; set; }
  public String? endereco_destino { get; set; }
  public String? cidade_destino { get; set; }
  public String? codigo_postal { get; set; }
  public TimeOnly? hora_inicio { get; set; }
  public TimeOnly? hora_final { get; set; }
  public DateTime? vencimento { get; set; }
  public TimeSpan? duracao_feito { get; set; }
  public TimeSpan? desloca_feito { get; set; }
  public String? tipo_atividade { get; set; }
  public Int64? servico { get; set; }
  public String? area_trabalho { get; set; }
  public String? codigos { get; set; }
  public String? id_viatura { get; set; }
  public Int32? id_motorista { get; set; }
  public Int32? id_ajudante { get; set; }
  public Int32? id_tecnico { get; set; }
  public String? observacao { get; set; }
  public String? bairro_destino { get; set; }
  public Int32? instalacao { get; set; }
  public String? complemento_destino { get; set; }
  public String? referencia_destino { get; set; }
  public String? tipo_servico { get; set; }
  public Double? debitos_cliente { get; set; }
  public TipoInstalacao? tipo_instalacao { get; set; }
  public TimeSpan? Desloca_estima { get; set; }
  public TimeSpan? duracao_estima { get; set; }
  public String? motivo_indisponibilidade { get; set; }
}
public enum TipoInstalacao { Monofasico, Bifasico, Trifasico }
public enum TipoStatus {cancelado, concluido, deslocando, iniciado, rejeitado, pendente}
