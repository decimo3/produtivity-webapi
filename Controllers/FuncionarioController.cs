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
        [HttpPost("Verificar")]
        [ActionName("PostVerificar")]
        public ActionResult PostFuncionario(FuncionarioVerificacao verificacao)
        {
          try
          {
            var auth = _context.funcionario.Where( x =>
              x.matricula == verificacao.matricula &&
              x.admissao == verificacao.admissao &&
              x.nome_colaborador.ToLower() == verificacao.nome_colaborador.ToLower() ).FirstOrDefault();
            if(auth is null) return NotFound();
            var character = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var characters = new char[32];
            var random = new Random();
            for (int i = 0; i < characters.Length; i++) {
              characters[i] = character[random.Next(character.Length)];
            }
            auth.palavra = new String(characters);
            _context.Entry(auth).State = EntityState.Modified;
            try
            {
              _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException erro)
            {
              Problem(erro.InnerException?.Message);
            }
            return Ok(new { auth.palavra });
          }
          catch (DbUpdateConcurrencyException erro)
          {
            return BadRequest(erro.InnerException?.Message);
          }
          catch (Exception ex)
          {
            return Problem(ex.Message);
          }
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
                Problem(erro.InnerException?.Message);
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
        public async Task<ActionResult<Funcionario>> PostFuncionario(FuncionarioCreate f)
        {
            if (_context.funcionario == null) return Problem("Entity set 'Database.funcionario'  is null.");
            if (FuncionarioExists(f.matricula)) return Conflict();
            var funcionario = new Funcionario()
            {
              matricula = f.matricula,
              nome_colaborador = f.nome_colaborador,
              admissao = f.admissao,
              funcao = f.funcao,
              regional = f.regional,
              atividade = f.atividade
            };
            _context.funcionario.Add(funcionario);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetFuncionario", new { id = funcionario.matricula }, funcionario);
            }
            catch (DbUpdateConcurrencyException erro)
            {
              return BadRequest(erro.InnerException?.Message);
            }
            catch (Exception ex)
            {
              return Problem(ex.Message);
            }
        }

        // DELETE: api/Funcionario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
          if (_context.funcionario == null) return NotFound();
          var funcionario = await _context.funcionario.FindAsync(id);
          if (funcionario == null) return NotFound();
          funcionario.situacao = SituacaoFuncionario.DESLIGADO;
          try
          {
            await _context.SaveChangesAsync();
            return NoContent();
          }
          catch (DbUpdateConcurrencyException erro)
          {
            return BadRequest(erro.InnerException?.Message);
          }
          catch (Exception ex)
          {
            return Problem(ex.Message);
          }
        }
        private bool FuncionarioExists(int id)
        {
            return (_context.funcionario?.Any(e => e.matricula == id)).GetValueOrDefault();
        }
        [HttpPut("TrocarSenha/{id}")]
        [ActionName("PutTrocarSenha")]
        public ActionResult PutFuncionario(Int32 id, FuncionarioTrocarSenha alterar)
        {
          var funcionario = _context.funcionario.Find(id);
          if(funcionario is null) return NotFound();
          if(funcionario.palavra != alterar.atual) return Unauthorized();
          if(alterar.nova != alterar.confirmacao) return BadRequest();
          var padrao = "^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,}$";
          var re = new System.Text.RegularExpressions.Regex(padrao);
          if(!re.IsMatch(alterar.nova)) return BadRequest();
          funcionario.palavra = alterar.nova;
          try
          {
            _context.SaveChanges();
            return NoContent();
          }
          catch (DbUpdateConcurrencyException erro)
          {
            return BadRequest(erro.InnerException?.Message);
          }
          catch (Exception ex)
          {
            return Problem(ex.Message);
          }
        }
    }
}
