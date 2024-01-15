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
        private readonly Database database;
        private readonly IHttpContextAccessor httpContext;
        private readonly AlteracoesServico alteracaoLog;
        public ComposicaoController(Database database, IHttpContextAccessor httpContext, AlteracoesServico alteracaoLog)
        {
            this.database = database;
            this.httpContext = httpContext;
            this.alteracaoLog = alteracaoLog;
            this.alteracaoLog.responsavel = ((Funcionario)httpContext.HttpContext!.Items["User"]!).matricula;
            this.alteracaoLog.tabela = this.ToString()!;
        }
        // GET: api/Composicao
        [HttpGet("{inicio}/{final}/{regional}/{atividade}")]
        public async Task<ActionResult<IEnumerable<Composicao>>> Getcomposicao(DateOnly inicio, DateOnly final, Regional regional, Atividade atividade)
        {
          if (_context.composicao == null) return NotFound();
          if (atividade == Atividade.NENHUM && regional == Regional.NENHUM) return await (from f in _context.composicao where (f.dia >= inicio) && (f.dia <= final) select f).ToListAsync();
          if (regional == Regional.NENHUM && atividade != Atividade.NENHUM) return await (from f in _context.composicao where (f.dia >= inicio) && (f.dia <= final) && (f.atividade == atividade) select f).ToListAsync();
          if (atividade == Atividade.NENHUM && regional != Regional.NENHUM) return await (from f in _context.composicao where (f.dia >= inicio) && (f.dia <= final) && (f.regional == regional) select f).ToListAsync();
          return await (from f in _context.composicao where (f.dia >= inicio) && (f.dia <= final) && (f.regional == regional) && (f.atividade == atividade) select f).ToListAsync();
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
        [HttpPost("Formulario")]
        [ActionName("PostFormulario")]
        public ActionResult PostComposicao(Composicao composicao)
        {
          if (_context.composicao == null)
          {
              return Problem("Entity set 'Database.composicao'  is null.");
          }
          if (ComposicaoExists(composicao.dia, composicao.recurso))
          {
              return Conflict();
          }
          _context.composicao.Add(composicao);
          try
          {
            _context.SaveChangesAsync();
            return Created("/Composicao", composicao);
          }
          catch (System.InvalidOperationException erro)
          {
            return BadRequest(erro.Message);
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
        [HttpPost("Arquivo")]
        [ActionName("PostArquivo")]
        public IActionResult PostComposicao(IFormFile file)
        {
            if (file.Length == 0) return BadRequest("O arquivo enviado está vazio!");
            var filemanager = new FileManager(_context, file);
            try
            {
              var composicoes = filemanager.Composicao();
              if (composicoes.GroupBy(a => a.dia).Any(b => b.Count() > 1))
                throw new InvalidOperationException("A composição enviada contém mais de uma data!\nA composição deve ser enviada um dia por vez.");
              if (composicoes.Where(c => c.validacao.Any()).Any())
              {
                return UnprocessableEntity(composicoes);
              }
              _context.AddRange(composicoes);
              _context.SaveChanges();
              return CreatedAtAction("GetComposicao", null, composicoes);
            }
            catch (System.InvalidOperationException erro)
            {
              return BadRequest(erro.Message);
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
