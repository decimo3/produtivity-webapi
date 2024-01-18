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
    private readonly AlteracoesServico alteracaoLog;
    public ContratoController(Database context, IHttpContextAccessor httpContext, AlteracoesServico alteracaoLog)
    {
        _context = context;
        this.alteracaoLog = alteracaoLog;
        this.alteracaoLog.responsavel = ((Funcionario)httpContext.HttpContext!.Items["User"]!).matricula;
        this.alteracaoLog.tabela = this.ToString()!;
    }
    private bool ContratoExists(Int64 id)
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
            alteracaoLog.Registrar("POST", null, contrato);
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
    public async Task<IActionResult> DeleteContrato(Int64 id)
    {
        if (_context.contrato == null) return NotFound();
        var contrato = await _context.contrato.FindAsync(id);
        if (contrato == null) return NotFound();
        _context.contrato.Remove(contrato);
        try
        {
            await _context.SaveChangesAsync();
            alteracaoLog.Registrar("DELETE", null, contrato);
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
