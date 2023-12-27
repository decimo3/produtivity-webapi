using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Services;
namespace backend.Controllers;
[ApiController]
[Route("[controller]")]
public class ObjetivosController : ControllerBase
{
    private readonly Database _context;
    public ObjetivosController(Database context)
    {
        _context = context;
    }
    private bool ObjetivosExists(Regional regional, TipoViatura tipoviatura, Atividade atividade)
    {
        return (_context.objetivo?.Any(e => e.regional == regional && e.tipo_viatura == tipoviatura && e.atividade == atividade)).GetValueOrDefault();
    }
    [HttpPost]
    public async Task<ActionResult<Objetivos>> PostObjetivos(Objetivos objetivos)
    {
        if (_context.objetivo == null) return Problem("Entity set 'Database.objetivo'  is null.");
        if (ObjetivosExists(objetivos.regional, objetivos.tipo_viatura, objetivos.atividade)) return Conflict();
        _context.objetivo.Add(objetivos);
        try
        {
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetObjetivos", new { id = objetivos.regional }, objetivos);
        }
        catch (DbUpdateConcurrencyException erro)
        {
            return Problem(erro.InnerException!.Message);
        }
        catch (Exception erro)
        {
            return Problem(erro.Message);
        }
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Objetivos>>> Getobjetivo()
    {
        if (_context.objetivo == null) return NotFound();
        return await _context.objetivo.ToListAsync();
    }
    [HttpDelete("{regional}/{viatura}/{atividade}")]
    public async Task<IActionResult> DeleteObjetivos(Regional regional, TipoViatura viatura, Atividade atividade)
    {
        if (_context.objetivo == null) return NotFound();
        var objetivos = await _context.objetivo.FindAsync(regional, viatura, atividade);
        if (objetivos == null) return NotFound();
        _context.objetivo.Remove(objetivos);
        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException erro)
        {
            return Problem(erro.InnerException!.Message);
        }
        catch (Exception erro)
        {
            return Problem(erro.Message);
        }
    }
}
