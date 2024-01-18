namespace backend.Models;
public class Processo
{
  public Int16 id_processo { get; set; }
  public String processo { get; set; }
  public Processo(Int16 id_processo, String processo)
  {
    this.id_processo = id_processo;
    this.processo = processo;
  }
}
