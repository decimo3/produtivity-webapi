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
        private readonly AlteracoesServico alteracaoLog;
        public ComposicaoController(Database database, IHttpContextAccessor httpContext, AlteracoesServico alteracaoLog)
        {
            this.database = database;
            this.alteracaoLog = alteracaoLog;
            this.alteracaoLog.tabela = this.ToString()!;
            if(httpContext.HttpContext != null)
            {
              var funcionario = (Funcionario?)httpContext.HttpContext.Items["User"];
              if(funcionario != null) this.alteracaoLog.responsavel = funcionario.matricula;
            }
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
            if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
            if (!ComposicaoExists(data, recurso)) return NotFound();
            if (recurso != composicao.recurso)
            {
                if (ComposicaoExists(composicao.dia, composicao.recurso)) return Conflict();
                var composicao_atual = database.composicao.Find(data, recurso);
                if (composicao_atual == null) return NotFound();
                database.composicao.Remove(composicao_atual);
                database.composicao.Add(composicao);
                try
                {
                  database.SaveChanges();
                  alteracaoLog.Registrar("PUT", composicao_atual, composicao);
                  return NoContent();
                }
                catch (DbUpdateConcurrencyException erro)
                {
                  return Problem(erro.InnerException?.Message);
                }
            }
            else
            {
                var composicao_atual = database.composicao.AsNoTracking()
                    .Where(o => o.dia == data && o.recurso == recurso).Single();
                database.Entry(composicao).State = EntityState.Modified;
                try
                {
                    database.SaveChanges();
                    alteracaoLog.Registrar("PUT", composicao_atual, composicao);
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException erro)
                {
                    return Problem(erro.InnerException?.Message);
                }
            }
        }
        // POST: api/Composicao
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Formulario")]
        [ActionName("PostFormulario")]
        public ActionResult PostComposicao(Composicao composicao)
        {
          if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
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
            if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
            if (database.composicao == null) return NotFound();
            var composicao = database.composicao.Find(data, recurso);
            if (composicao == null) return NotFound();
            database.composicao.Remove(composicao);
            database.SaveChanges();
            alteracaoLog.Registrar("DELETE", composicao, null);
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
            if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
            if (file.Length == 0) return BadRequest("O arquivo enviado está vazio!");
            var filemanager = new FileManager(database, file);
            try
            {
              var composicoes = filemanager.Composicao();
              if (composicoes.Select(a => a.dia).Distinct().Count() > 1)
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
