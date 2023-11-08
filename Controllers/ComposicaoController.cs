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
    public class ComposicaoController : ControllerBase
    {
        private readonly Database _context;

        public ComposicaoController(Database context)
        {
            _context = context;
        }

        // GET: api/Composicao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Composicao>>> Getcomposicao()
        {
          if (_context.composicao == null)
          {
              return NotFound();
          }
            return await _context.composicao.ToListAsync();
        }

        // GET: api/Composicao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Composicao>> GetComposicao(string id)
        {
          if (_context.composicao == null)
          {
              return NotFound();
          }
            var composicao = await _context.composicao.FindAsync(id);

            if (composicao == null)
            {
                return NotFound();
            }

            return composicao;
        }

        // PUT: api/Composicao/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComposicao(string id, Composicao composicao)
        {
            if (id != composicao.recurso)
            {
                return BadRequest();
            }

            _context.Entry(composicao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComposicaoExists(id))
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

        // POST: api/Composicao
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Composicao>> PostComposicao(Composicao composicao)
        {
          if (_context.composicao == null)
          {
              return Problem("Entity set 'Database.composicao'  is null.");
          }
            _context.composicao.Add(composicao);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ComposicaoExists(composicao.recurso))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetComposicao", new { id = composicao.recurso }, composicao);
        }

        // DELETE: api/Composicao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComposicao(string id)
        {
            if (_context.composicao == null)
            {
                return NotFound();
            }
            var composicao = await _context.composicao.FindAsync(id);
            if (composicao == null)
            {
                return NotFound();
            }

            _context.composicao.Remove(composicao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComposicaoExists(string id)
        {
            return (_context.composicao?.Any(e => e.recurso == id)).GetValueOrDefault();
        }
    }
}
