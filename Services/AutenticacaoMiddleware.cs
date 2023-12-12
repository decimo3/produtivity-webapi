/*
Classe responsável por verificar se há um tokem no cabeçalho da requisição e valida-lo
Caso não tenha um token, o usuário não será autenticado, e só poderá acessar rotas públicas
<method>AuthMiddleware - Contrutor, que para cada requisição para a api, será recebido um objeto
RequestDelegate que contem as informações da requisição, inclusive o cabeçalho de autorização.
<method>AttachUserToContext - Método responsável por verificar se há um tokem no cabeçalho da
requisição, valida-lo e anexa-lo ao contexto atual da requisição.
*/
namespace backend.Services;
public class AutenticacaoMiddleware
{
  private readonly RequestDelegate request;
  private readonly string segredo;
    public AutenticacaoMiddleware(RequestDelegate request)
  {
    this.request = request;
    this.segredo = System.Environment.GetEnvironmentVariable("SECRET_KEY");
    if (segredo is null) throw new InvalidOperationException("Environment variable SECRET_KEY is not set!");
  }
  private void AttachUserToContext(HttpContext context, AutenticacaoServico autenticacaoServico, string token)
  {
    try
    {
      var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
      var key = System.Text.Encoding.UTF8.GetBytes(segredo);
      var issuerSiginingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key);
      var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = issuerSiginingKey,
          ValidateIssuer = false,
          ValidateAudience = false,
          ClockSkew = TimeSpan.Zero
        };
      tokenHandler.ValidateToken(token, tokenValidationParameters, out Microsoft.IdentityModel.Tokens.SecurityToken validatedToken);
        var jwtToken = (System.IdentityModel.Tokens.Jwt.JwtSecurityToken)validatedToken;
        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "re").Value);
        // attach user to context on successful jwt validation
        context.Items["User"] = autenticacaoServico.GetById(userId);
    }
    catch (Exception ex)
    {
        System.Console.WriteLine(ex);
        // do nothing if jwt validation fails
        // user is not attached to context so request won't have access to secure routes
    }
  }
  public async Task Invoke(HttpContext context, AutenticacaoServico autenticacaoServico)
  {
    var token = context.Request.Cookies["MeuCookie"];
    if (token != null) AttachUserToContext(context, autenticacaoServico, token);
    await request(context);
  }
}
