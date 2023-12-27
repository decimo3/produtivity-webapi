using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Services;

namespace backend.Controllers;
[ApiController]
[Route("[controller]")]
public class ContratoController : ControllerBase
{
    private readonly Database _context;
    public ContratoController(Database context)
    {
        _context = context;
    }
    private bool ContratoExists(string id)
    {
        return (_context.contrato?.Any(e => e.contrato == id)).GetValueOrDefault();
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contrato>>> Getcontrato()
    {
        if (_context.contrato == null) return NotFound();
        return await _context.contrato.ToListAsync();
    }
    [HttpPost]
    public async Task<ActionResult<Contrato>> PostContrato(Contrato contrato)
    {
        if (_context.contrato == null) return Problem("Entity set 'Database.contrato'  is null.");
        if (ContratoExists(contrato.contrato)) return Conflict();
        _context.contrato.Add(contrato);
        try
        {
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetContrato", new { id = contrato.contrato }, contrato);
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
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContrato(string id)
    {
        if (_context.contrato == null) return NotFound();
        var contrato = await _context.contrato.FindAsync(id);
        if (contrato == null) return NotFound();
        _context.contrato.Remove(contrato);
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
