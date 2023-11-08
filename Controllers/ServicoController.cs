using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Services;

namespace backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicoController : ControllerBase
    {
        private readonly Database _context;

        public ServicoController(Database context)
        {
            _context = context;
        }

        // GET: api/Servico
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servico>>> Getrelatorio()
        {
          if (_context.relatorio == null)
          {
              return NotFound();
          }
            return await _context.relatorio.ToListAsync();
        }

        // GET: api/Servico/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servico>> GetServico(int id)
        {
          if (_context.relatorio == null)
          {
              return NotFound();
          }
            var servico = await _context.relatorio.FindAsync(id);

            if (servico == null)
            {
                return NotFound();
            }

            return servico;
        }

        // PUT: api/Servico/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServico(int id, Servico servico)
        {
            if (id != servico.indentificador)
            {
                return BadRequest();
            }

            _context.Entry(servico).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServicoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Servico
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Servico>> PostServico(Servico servico)
        {
          if (_context.relatorio == null)
          {
              return Problem("Entity set 'Database.relatorio'  is null.");
          }
            _context.relatorio.Add(servico);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServicoExists(servico.indentificador))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServico", new { id = servico.indentificador }, servico);
        }

        // DELETE: api/Servico/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServico(int id)
        {
            if (_context.relatorio == null)
            {
                return NotFound();
            }
            var servico = await _context.relatorio.FindAsync(id);
            if (servico == null)
            {
                return NotFound();
            }

            _context.relatorio.Remove(servico);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServicoExists(int id)
        {
            return (_context.relatorio?.Any(e => e.indentificador == id)).GetValueOrDefault();
        }
    }
}
