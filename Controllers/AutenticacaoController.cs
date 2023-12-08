namespace backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
[ApiController]
[Route("[controller]")]
public class AutenticacaoController : ControllerBase
{
  private readonly AutenticacaoServico autenticacao;
  private readonly IHttpContextAccessor context;
  public AutenticacaoController(AutenticacaoServico autenticacao, IHttpContextAccessor context)
  {
    this.autenticacao = autenticacao;
    this.context = context;
  }
  [HttpPost]
  public ActionResult Post(AutenticacaoRequisicao requisicao)
  {
    var auth = autenticacao.Authenticate(requisicao);
    if(auth is null) return Unauthorized();
    var options = new CookieOptions
    {
      Expires = DateTime.Now.AddDays(7),
      Secure = false, // define a cookie como seguro, somente será enviado em conexões HTTPS.
      HttpOnly = false, // define a cookie como acessível somente por HTTP, não pode ser acessado por JavaScript.
      Path = "/",
      Domain = "localhost"
    };
    context.HttpContext.Response.Cookies.Append("MeuCookie", auth.token, options);
    return Ok(auth);
  }
}
