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
    public class FuncionarioController : ControllerBase
    {
        private readonly Database _context;

        public FuncionarioController(Database context)
        {
            _context = context;
        }

        // GET: api/Funcionario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> Getfuncionario()
        {
          if (_context.funcionario == null)
          {
              return NotFound();
          }
            return await _context.funcionario.ToListAsync();
        }

        // GET: api/Funcionario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
        {
          if (_context.funcionario == null)
          {
              return NotFound();
          }
            var funcionario = await _context.funcionario.FindAsync(id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return funcionario;
        }

        // PUT: api/Funcionario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionario)
        {
          if (_context.funcionario == null)
          {
              return Problem("Entity set 'Database.funcionario'  is null.");
          }
            if (id != funcionario.matricula)
            {
              if (!FuncionarioExists(id))
              {
                return NotFound();
              }
              if (FuncionarioExists(funcionario.matricula))
              {
                return Conflict();
              }
              _context.funcionario.Add(funcionario);
              //
              var f = await _context.funcionario.FindAsync(id);
              if (f == null)
              {
                  return NotFound();
              }
              _context.funcionario.Remove(f);
              try
              {
                await _context.SaveChangesAsync();
              }
              catch (DbUpdateConcurrencyException erro)
              {
                Problem(erro.InnerException.Message);
              }
              return NoContent();
            }

            _context.Entry(funcionario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExists(id))
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

        // POST: api/Funcionario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Funcionario>> PostFuncionario(Funcionario funcionario)
        {
          if (_context.funcionario == null)
          {
              return Problem("Entity set 'Database.funcionario'  is null.");
          }
            _context.funcionario.Add(funcionario);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FuncionarioExists(funcionario.matricula))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFuncionario", new { id = funcionario.matricula }, funcionario);
        }

        // DELETE: api/Funcionario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            if (_context.funcionario == null)
            {
                return NotFound();
            }
            var funcionario = await _context.funcionario.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }

            _context.funcionario.Remove(funcionario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FuncionarioExists(int id)
        {
            return (_context.funcionario?.Any(e => e.matricula == id)).GetValueOrDefault();
        }
    }
}
