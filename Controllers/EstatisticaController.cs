using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
namespace backend.Controllers;
[ApiController]
[Route("")]
[Route("/Index")]
public class EstatisticaController : ControllerBase
{
  private readonly Database _context;
  public EstatisticaController(Database context)
  {
    _context = context;
  }
  [HttpGet("{inicio}/{final}/{regional}/{atividade}")]
  public ActionResult Index(DateOnly inicio, DateOnly final, Regional regional, Atividade atividade)
  {
    return Ok(new {message = "Works fine!"});
  }
}
