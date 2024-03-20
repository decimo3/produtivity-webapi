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
      Path = "/"
    };
    if(context.HttpContext is null) return Problem("Não foi possível acessar o contexto da requisição!");
    context.HttpContext.Response.Cookies.Append("MeuCookie", auth.token, options);
    return Ok(auth);
  }
  [HttpGet]
  [AutenticacaoAtributo]
  public ActionResult Get()
  {
    if(context.HttpContext is null) return Problem("Não foi possível acessar o contexto da requisição!");
    var auth = context.HttpContext.Items["User"];
    if (auth is null) throw new InvalidOperationException("The employee was not defined in the context of the request!");
    return Ok(auth);
  }
}
