using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Services;
namespace backend.Controllers;
[ApiController]
[Route("[controller]")]
public class FeriadoController : ControllerBase
{
    private readonly Database _context;
    public FeriadoController(Database context)
    {
        _context = context;
    }
    private bool FeriadoExists(DateOnly id)
    {
        return (_context.feriado?.Any(e => e.dia == id)).GetValueOrDefault();
    }
    [HttpPost]
    public async Task<ActionResult<Feriado>> PostFeriado(Feriado feriado)
    {
        if (_context.feriado == null) return Problem("Entity set 'Database.feriado'  is null.");
        if (FeriadoExists(feriado.dia)) return Conflict();
        _context.feriado.Add(feriado);
        try
        {
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFeriado", new { id = feriado.dia }, feriado);
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
    public async Task<ActionResult<IEnumerable<Feriado>>> Getferiado()
    {
        if (_context.feriado == null) return NotFound();
        return await _context.feriado.ToListAsync();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeriado(DateOnly id)
    {
        if (_context.feriado == null) return NotFound();
        var feriado = await _context.feriado.FindAsync(id);
        if (feriado == null) return NotFound();
        _context.feriado.Remove(feriado);
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
