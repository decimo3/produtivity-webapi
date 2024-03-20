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
        private readonly AlteracoesServico alteracaoLog;

        public FuncionarioController(Database context, IHttpContextAccessor httpContext, AlteracoesServico alteracaoLog)
        {
            _context = context;
            this.alteracaoLog = alteracaoLog;
            this.alteracaoLog.tabela = this.ToString()!;
            if(httpContext.HttpContext != null)
            {
              var funcionario = (Funcionario?)httpContext.HttpContext.Items["User"];
              if(funcionario != null) this.alteracaoLog.responsavel = funcionario.matricula;
            }
        }

        // GET: api/Funcionario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> Getfuncionario()
        {
          if (_context.funcionario == null) return NotFound();
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
            var anterior = auth;
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
              alteracaoLog.responsavel = auth.matricula;
              alteracaoLog.Registrar("PUT", anterior, auth);
            }
            catch (DbUpdateConcurrencyException erro)
            {
              Problem(erro.InnerException?.Message);
            }
            return Ok();
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuncionario(int id, Funcionario funcionario)
        {
            if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
            if (_context.funcionario == null) return Problem("Entity set 'Database.funcionario'  is null.");
            if (!FuncionarioExists(id)) return NotFound();
            if (id != funcionario.matricula)
            {
                if (FuncionarioExists(funcionario.matricula)) return Conflict();
                var f = await _context.funcionario.FindAsync(id);
                if(f is null) return NotFound();
                _context.funcionario.Add(funcionario);
                _context.funcionario.Remove(f);
                try
                {
                    await _context.SaveChangesAsync();
                    alteracaoLog.Registrar("PUT", f, funcionario);
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException erro)
                {
                    return Problem(erro.InnerException?.Message);
                }
            }
            else
            {
                var func = await _context.funcionario.FindAsync(id);
                _context.Entry(funcionario).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    alteracaoLog.Registrar("PUT", func, funcionario);
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException erro)
                {
                    return Problem(erro.InnerException?.Message);
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult<Funcionario>> PostFuncionario(FuncionarioCreate f)
        {
            if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
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
                alteracaoLog.Registrar("POST", null, funcionario);
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
          if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
          if (_context.funcionario == null) return NotFound();
          var funcionario = await _context.funcionario.FindAsync(id);
          if (funcionario == null) return NotFound();
          var anterior = funcionario;
          funcionario.situacao = SituacaoFuncionario.DESLIGADO;
          try
          {
            await _context.SaveChangesAsync();
            alteracaoLog.Registrar("DELETE", anterior, funcionario);
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
          if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
          var funcionario = _context.funcionario.Find(id);
          if(funcionario is null) return NotFound();
          var anterior = funcionario;
          if(funcionario.palavra != alterar.atual) return Unauthorized();
          if(alterar.nova != alterar.confirmacao) return BadRequest();
          var padrao = "^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,}$";
          var re = new System.Text.RegularExpressions.Regex(padrao);
          if(!re.IsMatch(alterar.nova)) return BadRequest();
          funcionario.palavra = alterar.nova;
          try
          {
            _context.SaveChanges();
            alteracaoLog.Registrar("PUT", anterior, funcionario);
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
