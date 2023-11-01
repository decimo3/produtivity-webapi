using backend.Services;
using Microsoft.AspNetCore.Mvc;
namespace backend.Controllers;
[ApiController]
[Route("[controller]")]
public class UploaderController : ControllerBase
{
    private readonly Database database;
    public UploaderController(Database _database)
    {
        this.database = _database;
    }
    [HttpPost]
    public IActionResult Post(IFormFile file)
    {
        var filemanager = new FileManager(this.database, file);
        if(filemanager.erros.Count > 0) return BadRequest(filemanager.erros);
        this.database.AddRange(filemanager.list);
        try
        {
          this.database.SaveChanges();
          return Created("/relatorio", null);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException erro)
        {
          return BadRequest(erro.InnerException.Message);
        }
        catch (System.Exception erro)
        {
          return Problem(erro.Message);
        }
    }
}
