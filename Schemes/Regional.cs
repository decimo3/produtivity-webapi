namespace backend.Models;
public class Regional
{
  public Int16 id_regional { get; set; }
  public String regional { get; set; }
  public Regional(Int16 id_regional, String regional)
  {
    this.id_regional = id_regional;
    this.regional = regional;
  }
}
