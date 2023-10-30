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
        var erros = new List<string>();
        string ext = Path.GetExtension(file.FileName);
        if(ext != ".xlsx" && ext != ".csv")
        {
          erros.Add("O tipo de arquivo enviado não é suportado!");
          return BadRequest(erros);
        }
        if(file.Length == 0)
        {
          erros.Add("O arquivo enviado está vazio!");
          return BadRequest(erros);
        }
        if(ext == ".xlsx")
        {
          var composicoes = FileManager.Composicao(file.OpenReadStream());
          this.database.composicao.AddRange(composicoes);
          this.database.SaveChanges();
        }
        if(ext == ".csv")
        {
          var servicos = FileManager.Relatorio(new StreamReader(file.OpenReadStream()));
          this.database.relatorio.AddRange(servicos);
          this.database.SaveChanges();
        }
        // TODO:
        return (erros.Count != 0) ? BadRequest(erros) : Ok(erros);
    }
}
