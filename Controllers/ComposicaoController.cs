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
          if (database.composicao == null) return NotFound();
          if (atividade == Atividade.NENHUM && regional == Regional.NENHUM) return await (from f in database.composicao where (f.dia >= inicio) && (f.dia <= final) select f).ToListAsync();
          if (regional == Regional.NENHUM && atividade != Atividade.NENHUM) return await (from f in database.composicao where (f.dia >= inicio) && (f.dia <= final) && (f.atividade == atividade) select f).ToListAsync();
          if (atividade == Atividade.NENHUM && regional != Regional.NENHUM) return await (from f in database.composicao where (f.dia >= inicio) && (f.dia <= final) && (f.regional == regional) select f).ToListAsync();
          return await (from f in database.composicao where (f.dia >= inicio) && (f.dia <= final) && (f.regional == regional) && (f.atividade == atividade) select f).ToListAsync();
        }
        // PUT: api/Composicao/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{data}/{recurso}")]
        public ActionResult PutComposicao(DateOnly data, string recurso, Composicao composicao)
        {
            if (!ComposicaoExists(data, recurso))
            {
                return NotFound();
            }
            var c = database.composicao.Find(data, recurso);
            if (c == null) return NotFound();
            if (recurso != composicao.recurso)
            {
                if (ComposicaoExists(composicao.dia, composicao.recurso))
                {
                    return Conflict();
                }
                database.composicao.Remove(c);
                database.composicao.Add(composicao);
                try
                {
                  database.SaveChanges();
                  alteracaoLog.Registrar("PUT", c, composicao);
                  return NoContent();
                }
                catch (DbUpdateConcurrencyException erro)
                {
                  return Problem(erro.InnerException?.Message);
                }
            }
            database.Entry(composicao).State = EntityState.Modified;
            try
            {
                database.SaveChanges();
                alteracaoLog.Registrar("PUT", c, composicao);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException erro)
            {
                return Problem(erro.InnerException?.Message);
            }
        }
        // POST: api/Composicao
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Formulario")]
        [ActionName("PostFormulario")]
        public ActionResult PostComposicao(Composicao composicao)
        {
          if (database.composicao == null)
          {
              return Problem("Entity set 'Database.composicao'  is null.");
          }
          if (ComposicaoExists(composicao.dia, composicao.recurso))
          {
              return Conflict();
          }
          database.composicao.Add(composicao);
          try
          {
            database.SaveChanges();
            alteracaoLog.Registrar("POST", null, composicao);
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
        public ActionResult DeleteComposicao(DateOnly data, string recurso)
        {
            if (database.composicao == null) return NotFound();
            var composicao = database.composicao.Find(data, recurso);
            if (composicao == null) return NotFound();
            database.composicao.Remove(composicao);
            database.SaveChanges();
            alteracaoLog.Registrar("DEL", composicao, null);
            return NoContent();
        }

        private bool ComposicaoExists(DateOnly data, string recurso)
        {
            return (database.composicao?.Any(e => e.recurso == recurso && e.dia == data)).GetValueOrDefault();
        }
        [HttpPost("Arquivo")]
        [ActionName("PostArquivo")]
        public ActionResult PostComposicao(IFormFile file)
        {
            if (file.Length == 0) return BadRequest("O arquivo enviado está vazio!");
            var filemanager = new FileManager(database, file);
            try
            {
              var composicoes = filemanager.Composicao();
              if (composicoes.GroupBy(a => a.dia).Any(b => b.Count() > 1))
                throw new InvalidOperationException("A composição enviada contém mais de uma data!\nA composição deve ser enviada um dia por vez.");
              if (composicoes.Where(c => c.validacao.Any()).Any()) return UnprocessableEntity(composicoes);
              database.AddRange(composicoes);
              database.SaveChanges();
              alteracaoLog.Registrar("POST", null, new {filename = file.FileName, linhas = composicoes.Count});
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
