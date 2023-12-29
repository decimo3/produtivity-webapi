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
        [HttpPost]
        public ActionResult PostServico(IFormFile file)
        {
            if (file.Length == 0) return BadRequest("O arquivo enviado está vazio!");
            var filemanager = new FileManager(_context, file);
            var relatorio = filemanager.Relatorio();
            var adicionado = 0;
            var atualizado = 0;
            try
            {
              foreach (var servico in relatorio)
              {
                  if(_context.relatorio.Any(s => s.indentificador == servico.indentificador))
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
              return CreatedAtAction("GetServico", null, new {adicionado, atualizado});
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
        [HttpDelete("{filename}")]
        public ActionResult DeleteServico(string filename)
        {
          try
          {
            var relatorio = _context.relatorio.Where(x => x.filename == filename);
            if(!relatorio.Any()) return NotFound();
            _context.relatorio.RemoveRange(relatorio);
            var stats = _context.relatorioEstatisticas.Find(filename);
            if (stats != null) _context.relatorioEstatisticas.Remove(stats);
            _context.SaveChanges();
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
