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
        [HttpGet("{data}/{recurso}")]
        public async Task<ActionResult<Composicao>> GetComposicao(DateOnly data, string recurso)
        {
          if (_context.composicao == null)
          {
              return NotFound();
          }
            var composicao = await _context.composicao.FindAsync(data, recurso);

            if (composicao == null)
            {
                return NotFound();
            }

            return composicao;
        }

        // PUT: api/Composicao/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{data}/{recurso}")]
        public async Task<IActionResult> PutComposicao(DateOnly data, string recurso, Composicao composicao)
        {
            if (!ComposicaoExists(data, recurso))
            {
                return NotFound();
            }
            if (recurso != composicao.recurso)
            {
                if (ComposicaoExists(composicao.dia, composicao.recurso))
                {
                    return Conflict();
                }
                var c = await _context.composicao.FindAsync(data, recurso);
                if (c == null) return NotFound();
                _context.composicao.Remove(c);
                _context.composicao.Add(composicao);
                try
                {
                  await _context.SaveChangesAsync();
                  return NoContent();
                }
                catch (DbUpdateConcurrencyException erro)
                {
                  return Problem(erro.InnerException?.Message);
                }
            }
            _context.Entry(composicao).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComposicaoExists(data, recurso))
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
                if (ComposicaoExists(composicao.dia, composicao.recurso))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetComposicao", new { dia = composicao.dia, recurso = composicao.recurso }, composicao);
        }

        // DELETE: api/Composicao/5
        [HttpDelete("{data}/{recurso}")]
        public async Task<IActionResult> DeleteComposicao(DateOnly data, string recurso)
        {
            if (_context.composicao == null)
            {
                return NotFound();
            }
            var composicao = await _context.composicao.FindAsync(data, recurso);
            if (composicao == null)
            {
                return NotFound();
            }

            _context.composicao.Remove(composicao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComposicaoExists(DateOnly data, string recurso)
        {
            return (_context.composicao?.Any(e => e.recurso == recurso && e.dia == data)).GetValueOrDefault();
        }
        [HttpPost(Name = "ComposicaoArquivo")]
        public IActionResult Post(IFormFile file)
        {
            var filemanager = new FileManager(_context, file);
            var composicoes = filemanager.Composicao();
            if(filemanager.erros.Count > 0) return BadRequest(filemanager.erros);
            _context.AddRange(composicoes);
            try
            {
              _context.SaveChanges();
              return CreatedAtAction("GetComposicao", null, composicoes);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException erro)
            {
              return BadRequest(erro.InnerException?.Message);
            }
            catch (System.Exception erro)
            {
              return Problem(erro.Message);
            }
        }
    }
}
