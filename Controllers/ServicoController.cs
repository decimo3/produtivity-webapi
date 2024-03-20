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
    [ApiController]
    [Route("[controller]")]
    public class ServicoController : ControllerBase
    {
        private readonly Database _context;
        private readonly AlteracoesServico alteracaoLog;
        public ServicoController(Database context, IHttpContextAccessor httpContext, AlteracoesServico alteracaoLog)
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
        [HttpPost]
        public ActionResult PostServico(IFormFile file)
        {
          if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
            if (file.Length == 0) return BadRequest("O arquivo enviado está vazio!");
            if(_context.relatorio.Any(r => r.filename == file.FileName))
                return Conflict("Já existe um relatório com esse nome de arquivo!\nSe for um reenvio, considere trocar o nome do arquivo ou excluir o relatório anterior!");
            var filemanager = new FileManager(_context, file);
            var relatorio = filemanager.Relatorio();
            var adicionado = 0;
            var atualizado = 0;
            try
            {
              foreach (var servico in relatorio)
              {
                  if(_context.relatorio.Any(s => s.serial == servico.serial))
                  {
                      _context.relatorio.Entry(servico).State = EntityState.Modified;
                      atualizado += 1;
                  }
                  else
                  {
                      _context.Add(servico);
                      adicionado += 1;
                  }
              }
              _context.SaveChanges();
              SetServico(file.FileName);
              var stats = _context.relatorioEstatisticas.Find(file.FileName);
              alteracaoLog.Registrar("POST", null, stats);
              return CreatedAtAction("GetServico", null, stats);
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
        private void SetServico(String filename)
        {
          var filtrado = _context.relatorio.Where(e => e.filename == filename);
          var relatorios = from f in filtrado orderby f.dia descending group f by f.filename into g orderby g.Key descending select new {filename = g.Key, servicos = g.Count(x => x.servico > 0 && x.status != TipoStatus.cancelado), recursos = g.Count(x => x.tipo_atividade == "Início de turno"), dia = g.First().dia};
          foreach (var relatorio in relatorios.ToList())
          {
            var stats = new RelatorioEstatisticas(relatorio.filename, relatorio.dia, relatorio.recursos, relatorio.servicos);
            var rel = _context.relatorioEstatisticas.Find(relatorio.filename);
            if(rel == null)
            {
              _context.relatorioEstatisticas.Add(stats);
            }
            else
            {
              _context.relatorioEstatisticas.Entry(stats).State = EntityState.Modified;
            }
          }
          _context.SaveChanges();
        }
        [HttpGet]
        public ActionResult<IEnumerable<RelatorioEstatisticas>> GetServico()
        {
          if (_context.relatorioEstatisticas == null) return NotFound();
          return _context.relatorioEstatisticas.ToList();
        }
        [HttpGet("{filename}")]
        public ActionResult DownloadServico(string filename)
        {
          if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
          try
          {
            var stream = System.IO.File.OpenRead(@$"./AppData/relatorios_ofs/{filename}");
            return new FileStreamResult(stream, "application/octet-stream");
          }
          catch (FileNotFoundException erro)
          {
            return NotFound(erro.Message);
          }
          catch (Exception erro)
          {
            return Problem(erro.Message);
          }
        }
        [HttpDelete("{filename}")]
        public ActionResult DeleteServico(string filename)
        {
          if(!this.alteracaoLog.is_ready()) return Unauthorized("Usuário não foi encontrado no contexto da solicitação!");
          try
          {
            var relatorio = _context.relatorio.Where(x => x.filename == filename);
            if(!relatorio.Any()) return NotFound();
            _context.relatorio.RemoveRange(relatorio);
            var stats = _context.relatorioEstatisticas.Find(filename);
            if (stats != null) _context.relatorioEstatisticas.Remove(stats);
            _context.SaveChanges();
            alteracaoLog.Registrar("DELETE", stats, null);
            return NoContent();
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
