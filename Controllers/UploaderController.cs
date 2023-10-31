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
    public ActionResult<List<string>> Post(IFormFile file)
    {
        var filemanager = new FileManager(file);
        if(filemanager.erros.Count > 0) return BadRequest(filemanager.erros);
        this.database.AddRange(filemanager.list);
        this.database.SaveChanges();
        return Ok(filemanager.erros);
    }
}
