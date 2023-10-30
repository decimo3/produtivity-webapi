using System.Runtime.Serialization;

public enum Atividade {
  CORTE = 0,
  RELIGA = 1,
  AVANCADO = 2,
  CAMINHAO = 3,
  EMERGENCIA = 4
}

public enum Regional
{
  BAIXADA = 0,
  [EnumMember(Value = "CAMPO GRANDE")]
  OESTE = 1,
  JACAREPAGUA = 1
}
