namespace backend.Services;
using backend.Models;
public class AutenticacaoServico
{
  private readonly Database database;
  public AutenticacaoServico(Database database)
  {
    this.database = database;
  }
    public AutenticacaoResposta? Authenticate(AutenticacaoRequisicao model)
    {
        try
        {
            var user = database.funcionario
              .Where(f => f.matricula == model.matricula && f.palavra == model.palavra)
              .Single();
            var token = JwtTokenGenerator.GenerateJwtToken(user);
            return new AutenticacaoResposta(user, token);
        }
        catch
        {
            return null;
        }
    }
  public Funcionario GetById(Int32 mat)
  {
    return database.funcionario.Where(f => f.matricula == mat).Single();
  }
}
